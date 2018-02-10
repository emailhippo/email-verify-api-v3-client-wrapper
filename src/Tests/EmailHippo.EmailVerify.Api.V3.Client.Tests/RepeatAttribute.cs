// <copyright file="RepeatAttribute.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;
    using Xunit.Sdk;

    /// <summary>
    /// Repeat attribute
    /// </summary>
    /// <seealso cref="Xunit.Sdk.DataAttribute" />
    public class RepeatAttribute : DataAttribute
    {
        private readonly int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatAttribute"/> class.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <exception cref="ArgumentOutOfRangeException">count - Repeat count must be greater than 0.</exception>
        public RepeatAttribute(int count)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Repeat count must be greater than 0.");
            }

            this.count = count;
        }

        /// <summary>
        /// Returns the data to be used to test the theory.
        /// </summary>
        /// <param name="testMethod">The method that is being tested</param>
        /// <returns>
        /// One or more sets of theory data. Each invocation of the test method
        /// is represented by a single object array.
        /// </returns>
        [ItemNotNull]
        [CanBeNull]
        public override IEnumerable<object[]> GetData([NotNull] MethodInfo testMethod)
        {
            return Enumerable.Repeat(new object[0], this.count);
        }
    }
}