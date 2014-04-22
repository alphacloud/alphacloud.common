#region copyright

// Copyright 2013-2014 Alphacloud.Net
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
    using AutoMapper;
    using JetBrains.Annotations;
    using NUnit.Framework;


    [PublicAPI]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ResetAutoMapperAttribute : Attribute, ITestAction
    {
        #region ITestAction Members

        /// <summary>
        ///   Executed before each test is run
        /// </summary>
        /// <param name="testDetails">Provides details about the test that is going to be run.</param>
        public void BeforeTest(TestDetails testDetails)
        {
            Mapper.Reset();
        }


        /// <summary>
        ///   Executed after each test is run
        /// </summary>
        /// <param name="testDetails">Provides details about the test that has just been run.</param>
        public void AfterTest(TestDetails testDetails)
        {
            Mapper.Reset();
        }


        /// <summary>
        ///   Provides the target for the action attribute
        /// </summary>
        /// <returns>
        ///   The target for the action attribute
        /// </returns>
        public ActionTargets Targets
        {
            get { return ActionTargets.Suite; }
        }

        #endregion
    }
}