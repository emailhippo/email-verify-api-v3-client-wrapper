// <copyright file="VerificationResponses.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Entities.Service.V3
{
    using System.Collections.ObjectModel;
    using JetBrains.Annotations;

    /// <summary>
    /// The verification responses.
    /// </summary>
    public sealed class VerificationResponses
    {
        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        [ItemNotNull]
        [CanBeNull]
        public ReadOnlyCollection<Api.V3.Entities.V_3_0_0.Result> Results { get; set; }
    }
}