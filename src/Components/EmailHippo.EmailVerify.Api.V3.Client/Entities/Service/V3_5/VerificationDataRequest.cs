// <copyright file="VerificationDataRequest.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Entities.Service.V3_5
{
    using JetBrains.Annotations;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using ProtoBuf;

    /// <summary>
    /// The verification data request.
    /// </summary>
    [ProtoContract]
    public sealed class VerificationDataRequest
    {
        /// <summary>
        /// Gets or sets the <see cref="ServiceType"/> of the service.
        /// </summary>
        /// <value>
        /// The <see cref="ServiceType"/> of the service.
        /// </value>
        /// <remarks>
        /// Default : ServiceType.More
        /// </remarks>
        [JsonProperty(Order = 1)]
        [JsonConverter(typeof(StringEnumConverter))]
        [ProtoMember(1)]
        public ServiceType ServiceType { get; set; } = ServiceType.More;

        /// <summary>
        /// Gets or sets the other data.
        /// </summary>
        /// <value>
        /// The other data.
        /// </value>
        [JsonProperty(Order = 2)]
        [ProtoMember(2)]
        [CanBeNull]
        public string OtherData { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [JsonProperty(Order = 3)]
        [ProtoMember(3)]
        [NotNull]
        public string EmailAddress { get; set; }
    }
}