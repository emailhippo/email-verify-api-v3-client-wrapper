// <copyright file="EventIds.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Diagnostics.Common
{
    /// <summary>
    /// The event IDs
    /// </summary>
    internal enum EventIds
    {
        /// <summary>
        ///     The none.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The initializing.
        /// </summary>
        Initializing = 1100,

        /// <summary>
        ///     The initialized
        /// </summary>
        Initialized = 1101,

        /// <summary>
        ///     The timer logging
        /// </summary>
        TimerLogging = 3100,

        /// <summary>
        ///     The critical
        /// </summary>
        Critical = 9901,

        /// <summary>
        ///     The warning
        /// </summary>
        Warning = 4501,

        /// <summary>
        ///     The verbose
        /// </summary>
        Verbose = 3502,

        /// <summary>
        ///     The error
        /// </summary>
        Error = 5901,

        /// <summary>
        /// The validation error
        /// </summary>
        ValidationError = 5910,

        /// <summary>
        ///     The informational
        /// </summary>
        Informational = 3501,

        /// <summary>
        ///     The log always
        /// </summary>
        LogAlways = 3503,

        /// <summary>
        ///     The method enter
        /// </summary>
        MethodEnter = 3101,

        /// <summary>
        ///     The method exit
        /// </summary>
        MethodExit = 3102,

        /// <summary>
        /// The HTTP get request.
        /// </summary>
        HttpGetRequest = 3103,

        /// <summary>
        /// The REST response
        /// </summary>
        RestResponse = 3104,

        /// <summary>
        /// The validation processor request
        /// </summary>
        ValidationProcessorRequest = 3105
    }
}