// <copyright file="DefaultClientTestsV3_5.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Tests.Integration.Logic.Clients.EmailHippo.V3_5
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Client.Logic.Clients.EmailHippo.V3_5;
    using Entities.Configuration.V3;
    using Entities.Service.V3_5;
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
    public sealed class DefaultClientTestsV3_5 : TestBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        [NotNull]
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultClientTestsV3_5"/> class.
        /// </summary>
        /// <param name="outHelper">The out helper.</param>
        public DefaultClientTestsV3_5([NotNull] ITestOutputHelper outHelper)
            : base(outHelper)
        {
            this.logger = this.LoggerFactory.CreateLogger<DefaultClientTestsV3_5>();
        }

#if !RELEASE
        /// <summary>
        /// Processes the asynchronous when valid email expect valid result.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="otherData">The other data.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [InlineData("abuse@hotmail.com", "some other data", ServiceType.More)]
        [InlineData("abuse@yahoo.com", "some other data 2", ServiceType.More)]
        [InlineData("syntax@syntax.com", "some other data 2", ServiceType.Syntax)]
#endif
        public async Task ProcessAsync_WhenValidEmail_ExpectValidResult([NotNull] string email, [CanBeNull] string otherData, ServiceType serviceType)
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration<KeyAuthentication>>();

            mockConfig.Setup(r => r.Get).Returns(() => new KeyAuthentication { LicenseKey = LicenseKey });

            var defaultClient = new DefaultClient(this.LoggerFactory, mockConfig.Object);

            // Act
            var stopwatch = Stopwatch.StartNew();
            var result = await defaultClient.ProcessAsync(new Entities.Clients.V3_5.VerificationRequest { Email = email, OtherData = otherData, ServiceType = serviceType }, CancellationToken.None).ConfigureAwait(false);
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