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
    using System.Threading;
    using Alphacloud.Common.Core.Instrumentation;
    using Alphacloud.Common.Infrastructure.Instrumentation;
    using FluentAssertions;
    using global::Castle.MicroKernel.Registration;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class InstrumentationRuntimeTests : IocTestBase
    {
        Mock<IInstrumentationContext> _instrumentationContext;
        Mock<ICorrelationIdProvider> _correlationIdProvider;
        Mock<IInstrumentationContextProvider> _instrumentationContextProvider;
        InstrumentationRuntime _instrumentationRuntime;
        InstrumentationSettings _instrumentationSettings;

        const string CorrelationId = "correlation.id";


        protected override void DoSetup()
        {
            base.DoSetup();

            _instrumentationContext = Mockery.Create<IInstrumentationContext>();
            _correlationIdProvider = Mockery.Create<ICorrelationIdProvider>();
            _instrumentationContextProvider = Mockery.Create<IInstrumentationContextProvider>();

            _correlationIdProvider.Setup(cp => cp.GetId()).Returns(CorrelationId);
            _instrumentationContextProvider.Setup(ic => ic.GetInstrumentationContext())
                .Returns(_instrumentationContext.Object);

            Container.Register(
                Component.For<ICorrelationIdProvider>().Instance(_correlationIdProvider.Object),
                Component.For<IInstrumentationContextProvider>().Instance(_instrumentationContextProvider.Object)
                );

            _instrumentationRuntime = new InstrumentationRuntime();
            _instrumentationSettings = new InstrumentationSettings {Enabled = true};
            _instrumentationRuntime.SetConfigurationProvider(() => _instrumentationSettings);
        }


        [Test]
        public void Attach_Should_AttachEventListener()
        {
            var listener = Mockery.Create<IInstrumentationEventListener>();

            _instrumentationRuntime.Attach(listener.Object);

            _instrumentationRuntime.OnCallCompleted(this, "svc", "service-method", 1.Seconds());
            listener.Verify(l => l.CallCompleted(this, It.IsAny<InstrumentationEventArgs>()));

            _instrumentationRuntime.OnOperationCompleted(this, "op", 1.Seconds());
            listener.Verify(l => l.OperationCompleted(this, It.IsAny<OperationCompletedEventArgs>()));
        }


        [Test]
        public void OnCallCompleted_Should_Broadcast_CallCompleted()
        {
            object sender = null;
            InstrumentationEventArgs args = null;

            _instrumentationRuntime.CallCompleted += (aSender, anArgs) => {
                sender = aSender;
                args = anArgs;
            };

            _instrumentationRuntime.OnCallCompleted(this, "svc", "service-method", 2.Seconds());

            sender.Should().Be(this, "failed to pass sender");

            args.Should().NotBeNull("event was not broadcasted");
            args.CallType.Should().Be("svc");
            args.Info.Should().Be("service-method");
            args.CorrelationId.Should().Be(CorrelationId);
            args.Duration.Should().Be(2.Seconds());
            args.ManagedThreadId.Should().Be(Thread.CurrentThread.ManagedThreadId);
        }


        [Test]
        public void OnOperationCompleted_Should_Broadcast_OperationCompleted()
        {
            object sender = null;
            OperationCompletedEventArgs args = null;

            _instrumentationRuntime.OperationCompleted += (aSender, anArgs) => {
                sender = aSender;
                args = anArgs;
            };

            _instrumentationRuntime.OnOperationCompleted(this, "operation", 5.Seconds());

            sender.Should().Be(this, "failed to pass sender");

            args.Should().NotBeNull("event was not broadcasted");
            args.Info.Should().Be("operation");
            args.CorrelationId.Should().Be(CorrelationId);
            args.Duration.Should().Be(5.Seconds());
            args.Context.Should().Be(_instrumentationContext.Object);
        }
    }
}