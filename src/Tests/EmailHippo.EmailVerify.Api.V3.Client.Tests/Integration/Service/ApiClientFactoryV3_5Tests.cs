// <copyright file="ApiClientFactoryV3_5Tests.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Tests.Integration.Service
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities.Service.V3_5;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Api Client Factory V3 Tests
    /// </summary>
    /// <seealso cref="TestBase" />
    public sealed class ApiClientFactoryV3_5Tests : TestBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        [NotNull]
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClientFactoryV3_5Tests"/> class.
        /// </summary>
        /// <param name="outHelper">The out helper.</param>
        public ApiClientFactoryV3_5Tests([NotNull] ITestOutputHelper outHelper)
            : base(outHelper)
        {
            this.logger = this.LoggerFactory.CreateLogger<ApiClientFactoryV3_5Tests>();

            ApiClientFactoryV3_5.Initialize(LicenseKey, this.LoggerFactory);
        }

        [ItemNotNull]
        [NotNull]
        private static IEnumerable<VerificationDataRequest> TestList1 => new List<VerificationDataRequest>
        {
            new VerificationDataRequest { EmailAddress = "abuse@hotmail.com", ServiceType = ServiceType.More, OtherData = "d1" },
            new VerificationDataRequest { EmailAddress = "abuse@aol.com", ServiceType = ServiceType.More, OtherData = "d2" },
            new VerificationDataRequest { EmailAddress = "abuse@yahoo.com", ServiceType = ServiceType.More, OtherData = "d3" },
            new VerificationDataRequest { EmailAddress = "abuse@bbc.co.uk", ServiceType = ServiceType.More, OtherData = "d4" },
            new VerificationDataRequest { EmailAddress = "abuse@mailinator.com", ServiceType = ServiceType.More, OtherData = "d5" },
            new VerificationDataRequest { EmailAddress = "abuse@abc.com", ServiceType = ServiceType.More, OtherData = "d6" },
            new VerificationDataRequest { EmailAddress = "abuse@microsoft.com", ServiceType = ServiceType.More, OtherData = "d7" },
            new VerificationDataRequest { EmailAddress = "abuse@gmail.com", ServiceType = ServiceType.More, OtherData = "d8" }
        };

        /// <summary>
        /// Gets the performance test list1.
        /// </summary>
        /// <value>
        /// The performance test list1.
        /// </value>
        [NotNull]
        [ItemNotNull]
        private static IEnumerable<VerificationDataRequest> PerformanceTestList1
        {
            get
            {
                const int returnedItems = 10;
                const string domainToTest = @"gmail.com";

                var rtn = new List<VerificationDataRequest>();

                for (int i = 0; i < returnedItems; i++)
                {
                    var randomFileName = Path.GetRandomFileName();

                    var concat = string.Concat(randomFileName, "@", domainToTest);

                    rtn.Add(new VerificationDataRequest { EmailAddress = concat });
                }

                return rtn;
            }
        }

#if !RELEASE
        /// <summary>
        /// Creates the and run work expect no errors.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
#endif
        public async Task CreateAndRunWork_ExpectNoErrors()
        {
            // arrange
            var service = ApiClientFactoryV3_5.Create();
            service.ProgressChanged += (o, args) => this.OutHelper.WriteLine(JsonConvert.SerializeObject(args));

            // act
            var stopwatch = Stopwatch.StartNew();
            var verificationResponses = await service.ProcessAsync(new VerificationRequest { VerificationData = TestList1 }, CancellationToken.None).ConfigureAwait(false);
            stopwatch.Stop();

            // assert
            Assert.True(verificationResponses != null);
            this.logger.LogInformation(JsonConvert.SerializeObject(verificationResponses));
            this.OutHelper.WriteLine(
                JsonConvert.SerializeObject(verificationResponses));
            this.WriteTimeElapsed(stopwatch.ElapsedMilliseconds);

            service.ProgressChanged -= (o, args) => this.OutHelper.WriteLine(JsonConvert.SerializeObject(args));
        }

#if !RELEASE
        /// <summary>
        /// Creates the and run performance test expect timings output only.
        /// </summary>
        [Fact]
#endif
        public void CreateAndRunPerformanceTest_ExpectTimingsOutputOnly()
        {
            // arrange
            var service = ApiClientFactoryV3_5.Create();
            service.ProgressChanged += (o, args) => this.OutHelper.WriteLine(JsonConvert.SerializeObject(args));

            // act
            var stopwatch = Stopwatch.StartNew();
            var verificationResponses = service.ProcessAsync(
                new VerificationRequest { VerificationData = PerformanceTestList1 },
                CancellationToken.None).Result;
            stopwatch.Stop();

            // assert
            Assert.True(verificationResponses != null);
            this.logger.LogInformation(JsonConvert.SerializeObject(verificationResponses));
            this.OutHelper.WriteLine(
                JsonConvert.SerializeObject(verificationResponses));
            this.WriteTimeElapsed(stopwatch.ElapsedMilliseconds);

            service.ProgressChanged -= (o, args) => this.OutHelper.WriteLine(JsonConvert.SerializeObject(args));
        }
    }
}