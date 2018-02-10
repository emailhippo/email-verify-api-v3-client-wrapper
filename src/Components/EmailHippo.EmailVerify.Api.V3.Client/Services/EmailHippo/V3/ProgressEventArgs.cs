// <copyright file="ProgressEventArgs.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Services.EmailHippo.V3
{
    using System;
    using Api.V3.Entities.V_3_0_0;
    using JetBrains.Annotations;

    /// <summary>
    /// Progress event args.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public sealed class ProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressEventArgs"/> class.
        /// </summary>
        /// <param name="totalCountInList">
        /// The total count in list.
        /// </param>
        /// <param name="percentageDone">
        /// The percentage done.
        /// </param>
        /// <param name="currentVerificationResponse">
        /// The current verification response.
        /// </param>
        public ProgressEventArgs(
            int totalCountInList,
            int percentageDone,
            [NotNull] Result currentVerificationResponse)
        {
            this.TotalCountInList = totalCountInList;
            this.PercentageDone = percentageDone;
            this.CurrentVerificationResponse = currentVerificationResponse;
        }

        /// <summary>
        ///     Gets the current verification response.
        /// </summary>
        /// <value>
        ///     The current verification response.
        /// </value>
        [NotNull]
        public Result CurrentVerificationResponse { get; }

        /// <summary>
        ///     Gets the percentage done.
        /// </summary>
        /// <value>
        ///     The percentage done.
        /// </value>
        public int PercentageDone { get; }

        /// <summary>
        ///     Gets the total count in list.
        /// </summary>
        /// <value>
        ///     The total count in list.
        /// </value>
        public int TotalCountInList { get; }
    }
}