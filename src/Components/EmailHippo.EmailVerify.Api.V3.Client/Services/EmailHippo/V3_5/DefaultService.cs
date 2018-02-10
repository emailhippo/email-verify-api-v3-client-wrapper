// <copyright file="DefaultService.cs" company="Email Hippo Ltd">
// (c) 2018, Email Hippo Ltd
// </copyright>

// Copyright 2018 Email Hippo Ltd
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace EmailHippo.EmailVerify.Api.V3.Client.Services.EmailHippo.V3_5
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using Diagnostics.Common;
    using Entities.Clients.V3_5;
    using Entities.Service.V3_5;
    using Helpers;
    using Interfaces.Clients;
    using Interfaces.Service;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Logging;
    using VerificationRequest = Entities.Service.V3_5.VerificationRequest;

    /// <summary>
    /// Default Service.
    /// </summary>
    /// <seealso cref="V3.ProgressEventArgs" />
    internal sealed class DefaultService : IService<VerificationRequest, VerificationResponses, V3.ProgressEventArgs>
    {
        /// <summary>
        /// The client proxy
        /// </summary>
        [NotNull]
        private readonly IClientProxy<Entities.Clients.V3_5.VerificationRequest, VerificationResponse> clientProxy;

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
            [NotNull] IClientProxy<Entities.Clients.V3_5.VerificationRequest, VerificationResponse> clientProxy)
        {
            this.clientProxy = clientProxy;

            this.logger = loggerFactory.CreateLogger<DefaultService>();
        }

        /// <inheritdoc />
        public event EventHandler<V3.ProgressEventArgs> ProgressChanged;

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

            List<VerificationDataRequest> verificationDataRequests = request.VerificationData.ToSafeEnumerable().ToList();

            var verificationRequests = verificationDataRequests.Select(r =>
                new Entities.Clients.V3_5.VerificationRequest
                {
                    Email = r.EmailAddress,
                    ServiceType = r.ServiceType,
                    OtherData = r.OtherData
                }).ToList();

            try
            {
                processLocalAsync =
                    await
                        this.ProcessLocalAsync(verificationRequests, cancellationToken)
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
        private static int CalculatePercentageProgress(int currentCountDone, int myTotalCount)
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
        /// <param name="data">The data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [ItemCanBeNull]
        private async Task<VerificationResponses> ProcessLocalAsync(
            [NotNull][ItemNotNull] IEnumerable<Entities.Clients.V3_5.VerificationRequest> data,
            CancellationToken cancellationToken)
        {
            var responses = new List<VerificationResponse>();

            var enumerable = data as IList<Entities.Clients.V3_5.VerificationRequest> ?? data.ToList();

            var totalCount = enumerable.Count;

            /*Consumer*/
            var actionBlock = new ActionBlock<Entities.Clients.V3_5.VerificationRequest>(
                async item =>
                {
                    var currentIndexCounter = 0;
                    Interlocked.Exchange(ref currentIndexCounter, 0);
                    VerificationResponse verificationResponse = null;

                    try
                    {
                        verificationResponse = await this.clientProxy.ProcessAsync(
                            new Entities.Clients.V3_5.VerificationRequest { ServiceType = item.ServiceType, Email = item.Email, OtherData = item.OtherData },
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
                            ServiceType = item.ServiceType,
                            OtherData = item.OtherData
                        };

                        responses.Add(response);
                        Interlocked.Increment(ref currentIndexCounter);

                        /*Progress calculations are meaningless for parallel processing therefore set to zero. In parallel mode, event will still return response*/
                        var i = CalculatePercentageProgress(currentIndexCounter, totalCount);

                        this.OnProgressChanged(new V3.ProgressEventArgs(totalCount, i, response.Result));
                    }
                    else
                    {
                        responses.Add(new VerificationResponse { OtherData = item.OtherData, ServiceType = item.ServiceType });
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

            var verificationDataResponses = new List<VerificationDataResponse>();

            foreach (var verificationResponse in responses)
            {
                verificationDataResponses.Add(new VerificationDataResponse { OtherData = verificationResponse.OtherData, Result = verificationResponse.Result, ServiceType = verificationResponse.ServiceType });
            }

            return new VerificationResponses { Results = new ReadOnlyCollection<VerificationDataResponse>(verificationDataResponses) };
        }

        /// <summary>
        /// Raises the <see cref="E:ProgressChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="V3.ProgressEventArgs" /> instance containing the event data.</param>
        private void OnProgressChanged(V3.ProgressEventArgs e)
        {
            var handler = this.ProgressChanged;
            handler?.Invoke(this, e);
        }
    }
}