// <copyright file="ClientProxyContracts.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Interfaces.Contracts
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Threading.Tasks;
    using Clients;

    /// <summary>
    /// Client Proxy Contracts
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="Clients.IClientProxy{TRequest, TResponse}" />
    [ContractClassFor(typeof(IClientProxy<,>))]
    internal abstract class ClientProxyContracts<TRequest, TResponse> : IClientProxy<TRequest, TResponse>
    {
        /// <inheritdoc />
        public virtual TResponse Process(TRequest request)
        {
            Contract.Requires(request != null);
            Contract.Ensures(Contract.Result<TResponse>() != null);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual Task<TResponse> ProcessAsync(TRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}