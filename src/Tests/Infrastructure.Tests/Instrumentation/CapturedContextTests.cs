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

namespace Infrastructure.Tests.Instrumentation
{
    using System.Globalization;
    using System.Security.Principal;
    using System.Threading;
    using Alphacloud.Common.Core.Instrumentation;
    using Alphacloud.Common.Core.Reflection;
    using Alphacloud.Common.Infrastructure.Instrumentation;
    using Alphacloud.Common.Testing.Nunit;
    using Alphacloud.Common.Testing.Nunit.Attributes;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;


    [TestFixture]
    [SetCulture("ru-RU")]
    [SetUICulture("uk-UA")]
    [SetPrincipal("capture-context")]
    class CapturedContextTests : MockedTestsBase
    {
        Mock<IInstrumentationContextProvider> _contextProvider;
        Mock<IInstrumentationContext> _instrumentationContext;
        Mock<IDiagnosticContext> _diagnosticContext;


        protected override void DoSetup()
        {
            InstrumentationRuntime.Reset();

            _contextProvider = Mockery.Create<IInstrumentationContextProvider>();
            _instrumentationContext = Mockery.Create<IInstrumentationContext>();
            _diagnosticContext = Mockery.Create<IDiagnosticContext>();
        }


        [Test]
        public void CaptureContext_Set_When_InstrumentationEnabled_Should_SetInstrumentationAndCorrelationId()
        {
            var culture = new CultureInfo("en-US");
            var uiCulture = new CultureInfo("en-GB");
            var principal = new GenericPrincipal(new GenericIdentity("captured"), new string[0]);
            var ctx = new CapturedContext(culture, uiCulture,
                principal, 11,
                "cid-001", _instrumentationContext.Object);

            ctx.Set();
            Thread.CurrentThread.CurrentCulture
                .Should().Be(culture);
            Thread.CurrentThread.CurrentUICulture
                .Should().Be(uiCulture);
            Thread.CurrentPrincipal.Should().Be(principal);
        }


        [Test]
        public void CaptureContext_When_InstrumentationEnabled_Should_CaptureInstrumentationAndCorrelationId()
        {
            InstrumentationRuntime.Instance.SetConfigurationProvider(() => new InstrumentationSettings(
                _contextProvider.Object,
                _diagnosticContext.Object
                ) {
                    Enabled = true,
                });

            _contextProvider.Setup(p => p.GetInstrumentationContext()).Returns(_instrumentationContext.Object)
                .Verifiable("instrumentation context was not captured");
            _diagnosticContext.Setup(p => p.Get()).Returns("correlation-id")
                .Verifiable("correlation id not was not captured");

            var context = InstrumentationRuntime.Instance.CaptureContext();

            context.FieldValue<string>("_correlationId").Should().Be("correlation-id");
            context.FieldValue<IInstrumentationContext>("_instrumentationContext")
                .Should().Be(_instrumentationContext.Object);
        }


        [Test]
        public void CaptureContext_When_Notinitialized_Should_CaptureCultureAndPrincipalOnly()
        {
            var context = InstrumentationRuntime.Instance.CaptureContext();

            context.Should().NotBeNull();
            context.FieldValue<CultureInfo>("_culture").Name.Should().Be("ru-RU");
            context.FieldValue<CultureInfo>("_uiCulture").Name.Should().Be("uk-UA");
            context.FieldValue<IPrincipal>("_principal").Identity.Name.Should().Be("capture-context");
            context.FieldValue<bool>("_instrumentationEnabled").Should().BeFalse();
            context.FieldValue<string>("_correlationId").Should().BeNull();
            context.FieldValue<IInstrumentationContext>("_instrumentationContext").Should().BeNull();

            _contextProvider.Verify(p => p.GetInstrumentationContext(), Times.Never(),
                "should not capture insurumentation context when Instrumentation Runtime is not enabled/initialized.");
            _diagnosticContext.Verify(p => p.Get(), Times.Never(),
                "should not capture correlation id when Instrumentation Runtime is not enabled/initialzed.");
        }


        [Test]
        public void CapturedContext_Set_Should_UpdatePrincipalAndCulture()
        {
            var culture = new CultureInfo("en-US");
            var uiCulture = new CultureInfo("en-GB");
            var ctx = new CapturedContext(culture, uiCulture,
                new GenericPrincipal(new GenericIdentity("captured"), new string[0]),
                Thread.CurrentThread.ManagedThreadId + 1);

            ctx.Set();

            Thread.CurrentThread.CurrentCulture.Should().Be(culture);
            Thread.CurrentThread.CurrentUICulture.Should().Be(uiCulture);
            Thread.CurrentPrincipal.Identity.Name.Should().Be("captured");
        }
    }
}