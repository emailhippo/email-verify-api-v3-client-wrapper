// <copyright file="KeyAuthentication.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Entities.Configuration.V3
{
    using JetBrains.Annotations;

    /// <summary>
    /// Key Authentication Config.
    /// </summary>
    public sealed class KeyAuthentication
    {
        /// <summary>
        /// Gets or sets the license key.
        /// </summary>
        /// <value>
        /// The license key.
        /// </value>
        [NotNull]
        public string LicenseKey { get; set; }
    }
}