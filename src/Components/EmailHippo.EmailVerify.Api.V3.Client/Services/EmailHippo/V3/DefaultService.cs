// <copyright file="DefaultService.cs" company="Email Hippo Ltd">
// (c) 2017, Email Hippo Ltd
// </copyright>

// Copyright 2017 Email Hippo Ltd
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace EmailHippo.EmailVerify.Api.V3.Client.Services.EmailHippo.V3
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using Api.V3.Entities.V_3_0_0;
    using Diagnostics.Common;
    using Entities.Clients.V3;
    using Entities.Service.V3;
    using Helpers;
    using Interfaces.Clients;
    using Interfaces.Service;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Logging;
    using VerificationRequest = Entities.Service.V3.VerificationRequest;

    /// <summary>
    /// Default Service.
    /// </summary>
    /// <seealso cref="ProgressEventArgs" />
    internal sealed class DefaultService : IService<VerificationRequest, VerificationResponses, ProgressEventArgs>
    {
        /// <summary>
        /// The client proxy
        /// </summary>
        [NotNull]
        private readonly IClientProxy<Entities.Clients.V3.VerificationRequest, VerificationResponse> clientProxy;

        /// <summary>
        /// The logger
        /// </summary>
        [NotNull]
        private readonly ILogger<DefaultService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultService" /> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="clientProxy">The client proxy.</param>
        public DefaultService(
            [NotNull] ILoggerFactory loggerFactory,
            [NotNull] IClientProxy<Entities.Clients.V3.VerificationRequest, VerificationResponse> clientProxy)
        {
            Contract.Requires(loggerFactory != null);
            Contract.Requires(clientProxy != null);

            this.clientProxy = clientProxy;

            this.logger = loggerFactory.CreateLogger<DefaultService>();
        }

        /// <inheritdoc />
        public event EventHandler<ProgressEventArgs> ProgressChanged;

        /// <inheritdoc />
        public VerificationResponses Process(VerificationRequest request)
        {
            return this.ProcessAsync(request, CancellationToken.None).Result;
        }

        /// <inheritdoc />
        public async Task<VerificationResponses> ProcessAsync(VerificationRequest request, CancellationToken cancellationToken)
        {
            if (this.logger.IsEnabled(LogLevel.Information))
            {
                this.logger.LogInformation((int)EventIds.MethodEnter, Messages.MethodEnter, @"ProcessAsync");
            }

            try
            {
                request.Validate();
            }
            catch (ValidationException exception)
            {
                this.logger.LogError((int)EventIds.Error, exception, Messages.ValidationError, string.Empty);
                throw;
            }

            var stopwatch = Stopwatch.StartNew();

            VerificationResponses processLocalAsync = null;

            var tuples = request.Emails.Select(item => new Tuple<ServiceType, string>(request.ServiceType, item)).ToList();

            try
            {
                processLocalAsync =
                    await
                        this.ProcessLocalAsync(tuples, cancellationToken)
                            .ConfigureAwait(false);
            }
            catch (AggregateException aggregateException)
            {
                aggregateException.Handle(
                    ae =>
                    {
                        this.logger.LogCritical((int)EventIds.Error, ae, Messages.ValidationError, string.Empty);
                        return false;
                    });
            }
            catch (Exception exception)
            {
                this.logger.LogCritical((int)EventIds.Critical, exception, Messages.ValidationError, string.Empty);
                throw;
            }
            finally
            {
                stopwatch.Stop();
            }

            if (!this.logger.IsEnabled(LogLevel.Information))
            {
                return processLocalAsync;
            }

            this.logger.LogInformation((int)EventIds.TimerLogging, Messages.TimerLogging, "ProcessAsync", stopwatch.ElapsedMilliseconds);
            this.logger.LogInformation((int)EventIds.MethodEnter, Messages.MethodExit, @"ProcessAsync");

            return processLocalAsync;
        }

        /// <summary>
        /// Calculates the percentage progress.
        /// </summary>
        /// <param name="currentCountDone">The current count done.</param>
        /// <param name="myTotalCount">My total count.</param>
        /// <returns>Percentage progress.</returns>
        internal static int CalculatePercentageProgress(int currentCountDone, int myTotalCount)
        {
            if (myTotalCount == 0
                || currentCountDone == 0)
            {
                return 0;
            }

            if (currentCountDone >= myTotalCount)
            {
                return 100;
            }

            var d = Convert.ToDouble(currentCountDone, CultureInfo.InvariantCulture);

            var d1 = Convert.ToDouble(myTotalCount, CultureInfo.InvariantCulture);

            var d2 = (d / d1) * 100;

            return (int)d2;
        }

        /// <summary>
        /// Processes the local asynchronous.
        /// </summary>
        /// <param name="emails">The emails.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [ItemCanBeNull]
        internal async Task<VerificationResponses> ProcessLocalAsync(
            [NotNull][ItemCanBeNull] IEnumerable<Tuple<ServiceType, string>> emails,
            CancellationToken cancellationToken)
        {
            Contract.Requires(emails != null);

            var responses = new List<VerificationResponse>();

            var enumerable = emails as IList<Tuple<ServiceType, string>> ?? emails.ToList();

            var totalCount = enumerable.Count;

            /*Consumer*/
            var actionBlock = new ActionBlock<Tuple<ServiceType, string>>(
                async item =>
                {
                    var currentIndexCounter = 0;
                    Interlocked.Exchange(ref currentIndexCounter, 0);
                    VerificationResponse verificationResponse = null;

                    try
                    {
                        verificationResponse = await this.clientProxy.ProcessAsync(
                            new Entities.Clients.V3.VerificationRequest { ServiceType = item.Item1, Email = item.Item2 },
                            cancellationToken).ConfigureAwait(false);
                    }
                    catch (AggregateException aggregateException)
                    {
                        aggregateException.Handle(
                            ae =>
                            {
                                this.logger.LogError((int)EventIds.Error, ae, Messages.ValidationError, string.Empty);
                                return true;
                            });
                    }
                    catch (Exception exception)
                    {
                        this.logger.LogError((int)EventIds.Error, exception, Messages.ValidationError, string.Empty);
                    }

                    if (verificationResponse != null)
                    {
                        var response = new VerificationResponse
                        {
                            Result = verificationResponse.Result,
                        };

                        responses.Add(response);
                        Interlocked.Increment(ref currentIndexCounter);

                        /*Progress calculations are meaningless for parallel processing therefore set to zero. In parallel mode, event will still return response*/
                        var i = CalculatePercentageProgress(currentIndexCounter, totalCount);

                        this.OnProgressChanged(new ProgressEventArgs(totalCount, i, response.Result));
                    }
                    else
                    {
                        this.logger.LogWarning((int)EventIds.Warning, "DefaultService.ProcessLocalAsync verificationResponse is null!");
                    }
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 2, CancellationToken = cancellationToken });

            /*Producer*/
            foreach (var email in enumerable)
            {
                actionBlock.Post(email);
            }

            actionBlock.Complete();

            try
            {
                await actionBlock.Completion.ConfigureAwait(false);
            }
            catch (AggregateException aggregateException)
            {
                aggregateException.Handle(
                    ae =>
                    {
                        this.logger.LogError((int)EventIds.Error, ae, Messages.ValidationError, string.Empty);
                        return false;
                    });
            }

            return new VerificationResponses { Results = new ReadOnlyCollection<Result>(responses.Select(r => r.Result).ToList()) };
        }

        private void OnProgressChanged(ProgressEventArgs e)
        {
            var handler = this.ProgressChanged;
            handler?.Invoke(this, e);
        }
    }
}