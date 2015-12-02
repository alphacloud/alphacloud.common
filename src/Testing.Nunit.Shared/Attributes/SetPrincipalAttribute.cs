#region copyright

// Copyright 2013 Alphacloud.Net
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

#endregion

namespace Alphacloud.Common.Testing.Nunit.Attributes
{
    using System;
    using System.Security;
    using System.Security.Principal;
    using System.Threading;
    using JetBrains.Annotations;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;

    /// <summary>
    ///   Sets custom principal for test execution.
    /// </summary>
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class SetPrincipalAttribute : Attribute, ITestAction
    {
        readonly string _authType;
        readonly string _identity;
        IPrincipal _oldPrincipal;


        /// <summary>
        ///   Specifies user identity to set.
        /// </summary>
        /// <param name="identity">The identity.</param>
        public SetPrincipalAttribute(string identity)
        {
            _identity = identity;
        }


        /// <summary>
        ///   Specifies user identity and authentication type.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="authType">The authentication type.</param>
        public SetPrincipalAttribute(string identity, string authType) : this(identity)
        {
            _authType = authType;
        }


        /// <summary>
        ///   User roles (optional).
        /// </summary>
        public string[] Roles { get; set; }

        #region ITestAction Members

        /// <summary>
        ///   Executed before each test is run
        /// </summary>
        /// <param name="test">The test that is going to be run.</param>
        /// <exception cref="SecurityException">The caller does not have the permission required to set the principal. </exception>
        public void BeforeTest(ITest test)
        {
            _oldPrincipal = Thread.CurrentPrincipal;
            var identity = string.IsNullOrEmpty(_authType)
                ? new GenericIdentity(_identity)
                : new GenericIdentity(_identity, _authType);

            var principal = new GenericPrincipal(identity, Roles);

            Thread.CurrentPrincipal = principal;
        }


        /// <summary>
        ///   Executed after each test is run
        /// </summary>
        /// <param name="test">The test that has just been run.</param>
        /// <exception cref="SecurityException">The caller does not have the permission required to set the principal. </exception>
        public void AfterTest(ITest test)
        {
            Thread.CurrentPrincipal = _oldPrincipal;
        }


        /// <summary>
        ///   Provides the target for the action attribute
        /// </summary>
        /// <returns>
        ///   The target for the action attribute
        /// </returns>
        public ActionTargets Targets => ActionTargets.Test;

        #endregion
    }
}