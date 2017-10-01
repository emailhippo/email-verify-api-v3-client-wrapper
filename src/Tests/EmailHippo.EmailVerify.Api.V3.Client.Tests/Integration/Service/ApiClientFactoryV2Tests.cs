// <copyright file="ApiClientFactoryV2Tests.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Tests.Integration.Service
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading;
    using Entities.Service.V3;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Api Client Factory V2 Tests
    /// </summary>
    /// <seealso cref="TestBase" />
    public sealed class ApiClientFactoryV2Tests : TestBase
    {

        private readonly ILogger logger;

        public ApiClientFactoryV2Tests([NotNull] ITestOutputHelper outHelper)
            : base(outHelper)
        {
            Contract.Requires(outHelper != null);

            this.logger = LoggerFactory.CreateLogger<ApiClientFactoryV2Tests>();

            ApiClientFactoryV3.Initialize(LicenseKey, this.LoggerFactory);
        }

        private static IEnumerable<string> TestList1 => new List<string>
        {
            "abuse@hotmail.com",
            "abuse@aol.com",
            "abuse@yahoo.com",
            "abuse@bbc.co.uk",
            "abuse@mailinator.com",
            "abuse@abc.com",
            "abuse@microsoft.com",
            "abuse@gmail.com"
        };

        /// <summary>
        /// Gets the performance test list1.
        /// </summary>
        /// <value>
        /// The performance test list1.
        /// </value>
        [NotNull]
        private static IEnumerable<string> PerformanceTestList1
        {
            get
            {
                Contract.Ensures(Contract.Result<List<string>>() != null);

                const int ReturnedItems = 10;
                const string DomainToTest = @"gmail.com";

                var rtn = new List<string>();

                for (int i = 0; i < ReturnedItems; i++)
                {
                    var randomFileName = Path.GetRandomFileName();

                    var concat = string.Concat(randomFileName, "@", DomainToTest);

                    rtn.Add(concat);
                }

                return rtn;
            }
        }

#if !RELEASE
        [Fact]
#endif
        public void CreateAndRunWork_ExpectNoErrors()
        {
            // arrange
            var service = ApiClientFactoryV3.Create();
            service.ProgressChanged += (o, args) => this.OutHelper.WriteLine(JsonConvert.SerializeObject(args));

            // act
            var stopwatch = Stopwatch.StartNew();
            var verificationResponses = service.ProcessAsync(
                new VerificationRequest { Emails = TestList1, ServiceType = ServiceType.More },
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

#if !RELEASE
        [Fact]
#endif
        public void CreateAndRunPerformanceTest_ExpectTimingsOutputOnly()
        {
            // arrange
            var service = ApiClientFactoryV3.Create();
            service.ProgressChanged += (o, args) => this.OutHelper.WriteLine(JsonConvert.SerializeObject(args));

            // act
            var stopwatch = Stopwatch.StartNew();
            var verificationResponses = service.ProcessAsync(
                new VerificationRequest { Emails = PerformanceTestList1, ServiceType = ServiceType.More },
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