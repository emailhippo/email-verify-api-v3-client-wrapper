// <copyright file="ServiceType.cs" company="Email Hippo Ltd">
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
namespace EmailHippo.EmailVerify.Api.V3.Client.Entities.Service.V3_5
{
    /// <summary>
    /// Service Type.
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// <para>No email validation performed.</para>
        ///  </summary>
        /// <remarks>
        /// <para>Functional Summary:</para>
        /// <list type="table">
        /// <listheader>
        /// <term>Operation</term>
        /// <description>Included (y/n)?</description>
        /// </listheader>
        /// <item>
        /// <term>Syntax to RFC 2822</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>DNS</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Blocklists (e.g. SpamHaus)</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Disposable Email Addresses (DEAs)</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Mail Box (SMTP Ping)</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Infrastructure Analysis (email and web)</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Spam Scoring</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Send Scoring</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Overall (Email Hippo) Scoring</term>
        /// <description>n</description>
        /// </item>
        /// </list>
        /// </remarks>
        None,

        /// <summary>
        /// <para>Syntax verification to RFC 2822.</para>
        /// <para>This is the fastest verification method.</para>
        ///  </summary>
        /// <remarks>
        /// <para>Functional Summary:</para>
        /// <list type="table">
        /// <listheader>
        /// <term>Operation</term>
        /// <description>Included (y/n)?</description>
        /// </listheader>
        /// <item>
        /// <term>Syntax to RFC 2822</term>
        /// <description>y</description>
        /// </item>
        /// <item>
        /// <term>DNS</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Blocklists (e.g. SpamHaus)</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Disposable Email Addresses (DEAs)</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Mail Box (SMTP Ping)</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Infrastructure Analysis (email and web)</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Spam Scoring</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Send Scoring</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Overall (Email Hippo) Quality Scoring</term>
        /// <description>n</description>
        /// </item>
        /// </list>
        /// </remarks>
        Syntax,

        /// <summary>
        /// <para>Syntax, DNS + spam blocklist verification.</para>
        /// <para>This is the second fastest verification method (slightly slower than syntax but faster than full / 'more' verification.</para>
        ///  </summary>
        /// <remarks>
        /// <para>Functional Summary:</para>
        /// <list type="table">
        /// <listheader>
        /// <term>Operation</term>
        /// <description>Included (y/n)?</description>
        /// </listheader>
        /// <item>
        /// <term>Syntax to RFC 2822</term>
        /// <description>y</description>
        /// </item>
        /// <item>
        /// <term>DNS</term>
        /// <description>y</description>
        /// </item>
        /// <item>
        /// <term>Blocklists (e.g. SpamHaus)</term>
        /// <description>y</description>
        /// </item>
        /// <item>
        /// <term>Disposable Email Addresses (DEAs)</term>
        /// <description>y</description>
        /// </item>
        /// <item>
        /// <term>Mail Box (SMTP Ping)</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Infrastructure Analysis (email and web)</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Spam Scoring</term>
        /// <description>y</description>
        /// </item>
        /// <item>
        /// <term>Send Scoring</term>
        /// <description>n</description>
        /// </item>
        /// <item>
        /// <term>Overall (Email Hippo) Quality Scoring</term>
        /// <description>n</description>
        /// </item>
        /// </list>
        /// </remarks>
        BlockLists,

        /// <summary>
        /// <para>Syntax, DNS, spam blocklist, mailbox verification + full scoring.</para>
        /// <para>This is the least fast but most comprehensive verification method.</para>
        ///  </summary>
        /// <remarks>
        /// <para>Functional Summary:</para>
        /// <list type="table">
        /// <listheader>
        /// <term>Operation</term>
        /// <description>Included (y/n)?</description>
        /// </listheader>
        /// <item>
        /// <term>Syntax to RFC 2822</term>
        /// <description>y</description>
        /// </item>
        /// <item>
        /// <term>DNS</term>
        /// <description>y</description>
        /// </item>
        /// <item>
        /// <term>Blocklists (e.g. SpamHaus)</term>
        /// <description>y</description>
        /// </item>
        /// <item>
        /// <term>Disposable Email Addresses (DEAs)</term>
        /// <description>y</description>
        /// </item>
        /// <item>
        /// <term>Mail Box (SMTP Ping)</term>
        /// <description>y</description>
        /// </item>
        /// <item>
        /// <term>Infrastructure Analysis (email and web)</term>
        /// <description>y</description>
        /// </item>
        /// <item>
        /// <term>Spam Scoring</term>
        /// <description>y</description>
        /// </item>
        /// <item>
        /// <term>Send Scoring</term>
        /// <description>y</description>
        /// </item>
        /// <item>
        /// <term>Overall (Email Hippo) Quality Scoring</term>
        /// <description>y</description>
        /// </item>
        /// </list>
        /// </remarks>
        More
    }
}