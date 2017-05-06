// <copyright file="TestBase.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Tests
{
    using System;
    using System.Diagnostics.Contracts;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Xunit.Abstractions;

    /// <summary>
    /// The Test Base.
    /// </summary>
    public abstract class TestBase
    {
        /// <summary>
        /// Logger factory
        /// </summary>
        private static readonly ILoggerFactory MyLoggerFactory = new LoggerFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="TestBase"/> class.
        /// </summary>
        /// <param name="outHelper">The out helper.</param>
        protected TestBase([NotNull] ITestOutputHelper outHelper)
        {
            Contract.Requires(outHelper != null);

            Log.Logger =
                new LoggerConfiguration()
                    .Enrich
                    .FromLogContext()
                    .WriteTo.LiterateConsole()
                    .CreateLogger();

            MyLoggerFactory
                .AddSerilog();

            this.OutHelper = outHelper;
        }

        /// <summary>
        /// Gets the License Key
        /// </summary>
        /// <remarks>
        /// Set system environment variable 'HippoAPILicKey' to a working, current license key.
        /// </remarks>
        protected static string LicenseKey => Environment.GetEnvironmentVariable("HippoAPILicKey");

        /// <summary>
        /// Gets the logger factory.
        /// </summary>
        protected ILoggerFactory LoggerFactory => MyLoggerFactory;

        /// <summary>
        /// Gets the out helper.
        /// </summary>
        /// <value>
        /// The out helper.
        /// </value>
        protected ITestOutputHelper OutHelper { get; }

        /// <summary>
        /// The write time elapsed.
        /// </summary>
        /// <param name="timerElapsed">
        /// The timer elapsed.
        /// </param>
        protected void WriteTimeElapsed(long timerElapsed)
        {
            this.OutHelper.WriteLine($"Elapsed timer: {timerElapsed}ms");
        }

        /// <summary>
        /// The write time elapsed.
        /// </summary>
        /// <param name="timerElapsed">
        /// The timer elapsed.
        /// </param>
        protected void WriteTimeElapsed(TimeSpan timerElapsed)
        {
            this.OutHelper.WriteLine($"Elapsed timer: {timerElapsed}");
        }
    }
}