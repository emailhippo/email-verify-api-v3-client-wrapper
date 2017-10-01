// <copyright file="DefaultClientTests.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Tests.Integration.Logic.Clients.EmailHippo.V3
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using Client.Logic.Clients.EmailHippo.V3;
    using Entities.Clients.V3;
    using Entities.Configuration.V3;
    using Interfaces.Configuration;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Newtonsoft.Json;
    using Tests;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Default Client Tests
    /// </summary>
    public sealed class DefaultClientTests : TestBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultClientTests"/> class.
        /// </summary>
        /// <param name="outHelper">The out helper.</param>
        public DefaultClientTests([NotNull] ITestOutputHelper outHelper)
            : base(outHelper)
        {
            Contract.Requires(outHelper != null);

            this.logger = this.LoggerFactory.CreateLogger<DefaultClientTests>();
        }

#if !RELEASE
        [Theory]
        [InlineData("abuse@hotmail.com")]
        [InlineData("abuse@yahoo.com")]
#endif
        public void ProcessAsync_WhenValidEmail_ExpectValidResult(string email)
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration<KeyAuthentication>>();

            mockConfig.Setup(r => r.Get).Returns(() => new KeyAuthentication { LicenseKey = LicenseKey });

            var defaultClient = new DefaultClient(this.LoggerFactory, mockConfig.Object);

            // Act
            var stopwatch = Stopwatch.StartNew();
            var result = defaultClient.ProcessAsync(new VerificationRequest { Email = email }, CancellationToken.None).Result;
            stopwatch.Stop();

            // Assert
            Assert.True(result.Result != null);
            this.logger.LogInformation("Result:{0}", JsonConvert.SerializeObject(result));
            this.OutHelper.WriteLine("Result:{0}", JsonConvert.SerializeObject(result));
            this.OutHelper.WriteLine(string.Empty);
            this.OutHelper.WriteLine(string.Empty);
            this.WriteTimeElapsed(stopwatch.ElapsedMilliseconds);
            this.OutHelper.WriteLine(string.Empty);
            this.OutHelper.WriteLine(string.Empty);
        }
    }
}