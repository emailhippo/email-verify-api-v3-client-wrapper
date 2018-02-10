// <copyright file="IClientProxy.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Interfaces.Clients
{
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    /// <summary>
    /// Client proxy interface.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    internal interface IClientProxy<in TRequest, TResponse>
    {
        /// <summary>
        /// Processes the specified request.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The Response.
        /// </returns>
        [NotNull]
        TResponse Process([NotNull] TRequest request);

        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [ItemCanBeNull]
        [NotNull]
        Task<TResponse> ProcessAsync([NotNull] TRequest request, CancellationToken cancellationToken);
    }
}