﻿// <copyright file="VerificationRequest.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Entities.Clients.V3
{
    using System.ComponentModel.DataAnnotations;
    using JetBrains.Annotations;
    using Service.V3;

    /// <summary>
    /// The verification request
    /// </summary>
    [System.Obsolete("Deprecated and may not be supported in future versions. Please use replacement type from V3_5 namespace.")]
    internal sealed class VerificationRequest
    {
        /// <summary>
        /// Gets or sets the service type.
        /// </summary>
        public ServiceType ServiceType { get; set; } = ServiceType.More;

        /// <summary>
        /// Gets or sets the emails.
        /// </summary>
        [MaxLength(255)]
        [EmailAddress]
        [Required]
        [CanBeNull]
        public string Email { get; set; }
    }
}