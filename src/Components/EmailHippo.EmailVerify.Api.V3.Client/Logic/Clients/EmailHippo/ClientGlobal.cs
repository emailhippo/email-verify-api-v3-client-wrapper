// <copyright file="ClientGlobal.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Logic.Clients.EmailHippo
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using JetBrains.Annotations;

    /// <summary>
    /// Client Global
    /// </summary>
    internal static class ClientGlobal
    {
        /// <summary>
        /// The client lazy
        /// </summary>
        [ItemNotNull]
        [CanBeNull]
        private static Lazy<HttpClient> clientLazy;

        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        /// <value>
        /// The HTTP client.
        /// </value>
        [NotNull]
        public static HttpClient HttpClient
        {
            get
            {
                if (clientLazy != null)
                {
                    return clientLazy.Value;
                }

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));

                clientLazy = new Lazy<HttpClient>(() => httpClient);

                return clientLazy.Value;
            }
        }
    }
}