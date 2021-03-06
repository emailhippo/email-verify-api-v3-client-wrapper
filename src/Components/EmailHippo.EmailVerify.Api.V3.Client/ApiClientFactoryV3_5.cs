﻿// <copyright file="ApiClientFactoryV3_5.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client
{
    using System;
    using System.Threading;
    using Entities.Configuration.V3;
    using Entities.Service.V3_5;
    using Interfaces.Service;
    using JetBrains.Annotations;
    using Logic.Clients.EmailHippo.V3_5;
    using Microsoft.Extensions.Logging;
    using Services.EmailHippo.V3_5;

    /// <summary>
    /// The API client factory for V3.5
    /// </summary>
    public static class ApiClientFactoryV3_5
    {
        /// <summary>
        ///     The default client lazy.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        private static readonly Lazy<DefaultClient> DefaultClientLazy = new Lazy<DefaultClient>(() => new DefaultClient(myLoggerFactory, KeyAuthenticationLazy.Value));

        /// <summary>
        ///     The default service lazy.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        private static readonly Lazy<DefaultService> DefaultServiceLazy = new Lazy<DefaultService>(() => new DefaultService(myLoggerFactory, DefaultClientLazy.Value));

        [ItemNotNull]
        [NotNull]
        private static readonly Lazy<Logic.Configuration.V3.KeyAuthentication> KeyAuthenticationLazy =
            new Lazy<Logic.Configuration.V3.KeyAuthentication>(
                () =>
                    new Logic.Configuration.V3.KeyAuthentication
                    {
                        Get = new KeyAuthentication { LicenseKey = appDomainLicenseKey }
                    });

        /// <summary>
        ///     The app domain license key.
        /// </summary>
        [NotNull]
        private static string appDomainLicenseKey;

        /// <summary>
        /// My logger factory
        /// </summary>
        [NotNull]
        private static ILoggerFactory myLoggerFactory;

        /// <summary>
        ///     The initialized.
        /// </summary>
        private static long initialized;

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>The service.</returns>
        /// <exception cref="InvalidOperationException">License key not set. Call Initialize method first and either add key to appSettings, key='Hippo.EmailVerifyApiKey' or supply licenseKey parameter to Initialize(licenseKey) method.</exception>
        [NotNull]
        public static IService<VerificationRequest, VerificationResponses, Services.EmailHippo.V3.ProgressEventArgs> Create()
        {
            if (Interlocked.Read(ref initialized) < 1)
            {
                throw new InvalidOperationException(
                    "License key not set. Call Initialize method first and either add key to appSettings, key='Hippo.EmailVerifyApiKey' or supply licenseKey parameter to Initialize(licenseKey) method.");
            }

            return DefaultServiceLazy.Value;
        }

        /// <summary>
        /// Initializes the software.
        /// <remarks>
        /// This needs to be called only once per app domain.
        /// </remarks>
        /// </summary>
        /// <param name="licenseKey">License key.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <exception cref="ArgumentNullException">licenseKey - License Key is required. Please visit www.emailhippo.com to get a free trial license.</exception>
        public static void Initialize([NotNull] string licenseKey, [CanBeNull] ILoggerFactory loggerFactory = null)
        {
            if (Interlocked.Read(ref initialized) > 0)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(licenseKey))
            {
                throw new ArgumentNullException(nameof(licenseKey), "License Key is required. Please visit www.emailhippo.com to get a free trial license.");
            }

            if (!string.IsNullOrWhiteSpace(licenseKey))
            {
                appDomainLicenseKey = licenseKey;
            }

            myLoggerFactory = loggerFactory ?? new LoggerFactory();

            Interlocked.Exchange(ref initialized, 1);
        }
    }
}