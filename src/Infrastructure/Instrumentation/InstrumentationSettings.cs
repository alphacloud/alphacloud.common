﻿#region copyright

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

namespace Alphacloud.Common.Infrastructure.Instrumentation
{
    using System;
    using Core.Instrumentation;
    using JetBrains.Annotations;


    /// <summary>
    ///   Instrumentation runtime settings.
    /// </summary>
    public class InstrumentationSettings
    {
        readonly IDiagnosticContext _diagnosticContext;
        readonly IInstrumentationContextProvider _instrumentationContextProvider;


        /// <summary>
        ///   Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public InstrumentationSettings([NotNull] IInstrumentationContextProvider instrumentationContextProvider,
            [NotNull] IDiagnosticContext diagnosticContext)
        {
            if (instrumentationContextProvider == null)
                throw new ArgumentNullException("instrumentationContextProvider");
            if (diagnosticContext == null) throw new ArgumentNullException("diagnosticContext");

            _instrumentationContextProvider = instrumentationContextProvider;
            _diagnosticContext = diagnosticContext;
        }


        public bool Enabled { get; set; }

        public IInstrumentationContextProvider InstrumentationContextProvider
        {
            get { return _instrumentationContextProvider; }
        }

        public IDiagnosticContext DiagnosticContext
        {
            get { return _diagnosticContext; }
        }
    }
}