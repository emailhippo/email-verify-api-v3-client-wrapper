// <copyright file="DefaultClient.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Logic.Clients.EmailHippo.V3_5
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Api.V3.Entities.V_3_0_0;
    using Diagnostics.Common;
    using Entities.Clients.V3_5;
    using Entities.Configuration.V3;
    using Entities.Service.V3_5;
    using Helpers;
    using Interfaces.Clients;
    using Interfaces.Configuration;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Logging;
    using ProtoBuf;

    /// <summary>
    /// The default client. Uses protobuf endpoint for speed and efficiency.
    /// </summary>
    internal sealed class DefaultClient : IClientProxy<Entities.Clients.V3_5.VerificationRequest, VerificationResponse>
    {
        /// <summary>
        /// The API url.
        /// </summary>
        private const string ApiUrlFormat = @"https://api.hippoapi.com/v3/{0}/proto/{1}/{2}";

        /// <summary>
        /// The API URL format (syntax checking only)
        /// </summary>
        private const string ApiUrlFormatSyntaxOnly = @"https://api.hippoapi.com/v3/{0}/proto/{1}";

        /// <summary>
        /// The logger
        /// </summary>
        [NotNull]
        private readonly ILogger<DefaultClient> logger;

        /// <summary>
        /// The authentication configuration
        /// </summary>
        [NotNull]
        private readonly IConfiguration<KeyAuthentication> authConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultClient" /> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="authConfiguration">The authentication configuration.</param>
        public DefaultClient(
            [NotNull] ILoggerFactory loggerFactory,
            [NotNull] IConfiguration<KeyAuthentication> authConfiguration)
        {
            this.logger = loggerFactory.CreateLogger<DefaultClient>();
            this.authConfiguration = authConfiguration;
        }

        /// <inheritdoc />
        public VerificationResponse Process(Entities.Clients.V3_5.VerificationRequest request)
        {
            return this.ProcessAsync(request, CancellationToken.None).Result;
        }

        /// <inheritdoc />
        public async Task<VerificationResponse> ProcessAsync(Entities.Clients.V3_5.VerificationRequest request, CancellationToken cancellationToken)
        {
            if (this.logger.IsEnabled(LogLevel.Information))
            {
                this.logger.LogInformation((int)EventIds.MethodEnter, Messages.MethodEnter, @"ProcessAsync");
                this.logger.LogInformation((int)EventIds.ValidationProcessorRequest, Messages.ValidationProcessorRequest, this.authConfiguration.Get.LicenseKey, request.Email);
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

            string requestUrl;

            var stopwatch = Stopwatch.StartNew();

            switch (request.ServiceType)
            {
                case ServiceType.None:
                    throw new NotImplementedException("service type = 'None' not implemented");
                case ServiceType.Syntax:
                    requestUrl = string.Format(ApiUrlFormatSyntaxOnly, "basic", request.Email);
                    break;
                case ServiceType.BlockLists:
                    requestUrl = string.Format(ApiUrlFormat, "blocklists", this.authConfiguration.Get.LicenseKey, request.Email);
                    break;
                case ServiceType.More:
                    requestUrl = string.Format(ApiUrlFormat, "more", this.authConfiguration.Get.LicenseKey, request.Email);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Result deserializeResult = null;

            var response = await ClientGlobal.HttpClient.GetAsync(new Uri(requestUrl), cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    try
                    {
                        deserializeResult = Serializer.Deserialize<Result>(stream);
                    }
                    catch (Exception exception)
                    {
                        this.logger.LogError((int)EventIds.Error, exception, Messages.Error, "deserialization error");
                        throw;
                    }
                }
            }

            var responseResult = new VerificationResponse { Result = deserializeResult, ServiceType = request.ServiceType, OtherData = request.OtherData };

            stopwatch.Stop();

            if (!this.logger.IsEnabled(LogLevel.Information))
            {
                return responseResult;
            }

            this.logger.LogInformation((int)EventIds.TimerLogging, Messages.TimerLogging, "ProcessAsync", stopwatch.ElapsedMilliseconds);
            this.logger.LogInformation((int)EventIds.MethodEnter, Messages.MethodExit, @"ProcessAsync");

            return responseResult;
        }
    }
}