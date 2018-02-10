// <copyright file="VerificationResponse.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Entities.Clients.V3_5
{
    using JetBrains.Annotations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Service.V3_5;

    /// <summary>
    /// The verification response.
    /// </summary>
    internal sealed class VerificationResponse
    {
        /// <summary>
        /// Gets or sets the service type.
        /// </summary>
        [JsonProperty(Order = 1)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ServiceType ServiceType { get; set; }

        /// <summary>
        /// Gets or sets the other data.
        /// </summary>
        /// <value>
        /// The other data.
        /// </value>
        [JsonProperty(Order = 2)]
        [CanBeNull]
        public string OtherData { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        [JsonProperty(Order = 3)]
        [CanBeNull]
        public Api.V3.Entities.V_3_0_0.Result Result { get; set; }
    }
}