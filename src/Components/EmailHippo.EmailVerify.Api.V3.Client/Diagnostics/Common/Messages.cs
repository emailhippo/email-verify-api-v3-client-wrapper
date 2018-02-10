// <copyright file="Messages.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Diagnostics.Common
{
    /// <summary>
    /// Diagnostic messages.
    /// </summary>
    internal static class Messages
    {
        /// <summary>
        ///     The critical
        /// </summary>
        public const string Critical = @"Error:{0}";

        /// <summary>
        ///     The error
        /// </summary>
        public const string Error = @"Error:{0}";

        /// <summary>
        ///     The informational
        /// </summary>
        public const string Informational = @"Message:{0}";

        /// <summary>
        ///     The initialized.
        /// </summary>
        public const string Initialized = @"Initialization complete.";

        /// <summary>
        ///     The initializing.
        /// </summary>
        public const string Initializing = @"Initializing.";

        /// <summary>
        ///     The log always
        /// </summary>
        public const string LogAlways = @"Message:{0}";

        /// <summary>
        ///     The method enter
        /// </summary>
        public const string MethodEnter = @"MethodEnter.Source:{0}";

        /// <summary>
        ///     The method exit
        /// </summary>
        public const string MethodExit = @"MethodExit. Source:{0}";

        /// <summary>
        ///     The timer logging
        /// </summary>
        public const string TimerLogging = @"Timer logging. Source:{0}, Elapsed:{1}ms";

        /// <summary>
        ///     The verbose
        /// </summary>
        public const string Verbose = @"Message:{0}";

        /// <summary>
        ///     The warning.
        /// </summary>
        public const string Warning = @"Message:{0}";

        /// <summary>
        /// The HTTP get request.
        /// </summary>
        public const string HttpGetRequest = @"HttpGetRequest. Url:{0}";

        /// <summary>
        /// The REST response
        /// </summary>
        public const string RestResponse = @"RestResponse. RestResponse:{0}";

        /// <summary>
        /// The validation processor request
        /// </summary>
        public const string ValidationProcessorRequest = @"Validation Processor Request. Key:{0}, WI:{1}";

        /// <summary>
        /// The validation error
        /// </summary>
        public const string ValidationError = @"Validation error. Please see exception stack trace for details. Additional info:{0}";
    }
}
