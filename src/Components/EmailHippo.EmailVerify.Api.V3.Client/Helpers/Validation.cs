// <copyright file="Validation.cs" company="Email Hippo Ltd">
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

namespace EmailHippo.EmailVerify.Api.V3.Client.Helpers
{
    using System.ComponentModel.DataAnnotations;

    internal static class Validation
    {
        /// <summary>
        /// The validate.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <typeparam name="T">
        /// Type of object to validate.
        /// </typeparam>
        public static void Validate<T>(this T item)
            where T : class
        {
            if (item == null)
            {
                return;
            }

            var validationContext = new ValidationContext(item);

            Validator.ValidateObject(item, validationContext);
        }
    }
}