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

using Microsoft.Owin;
using WebMvcSample;

[assembly: OwinStartup(typeof (Startup))]

namespace WebMvcSample
{
    using System.Web.Mvc;
    using Alphacloud.Common.Infrastructure.Instrumentation;
    using Alphacloud.Common.Instrumentation.Log4Net;
    using Alphacloud.Common.Web.Mvc.Instrumentation;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Common.Logging;
    using Controllers;
    using JetBrains.Annotations;
    using Owin;


    public class Startup
    {
        static readonly ILog s_log = LogManager.GetCurrentClassLogger();
        internal static IContainer Container;


        [UsedImplicitly]
        public void Configuration(IAppBuilder app)
        {
            var cb = new ContainerBuilder();
            ConfigureIoc(cb);
            Container = cb.Build();

            var dependencyResolver = new AutofacDependencyResolver(Container);
            DependencyResolver.SetResolver(dependencyResolver);

            ConfigureInstrumentation();
        }


        static void ConfigureInstrumentation()
        {
            var settings = new InstrumentationSettings(
                new HttpInstrumentationContextProvider(),
                new HttpCorrelationIdProvider(new Log4NetDiagnosticContext())
                ) {
                    Enabled = true,
                };
            InstrumentationRuntime.Instance.SetConfigurationProvider(() => settings);
        }


        static void ConfigureIoc(ContainerBuilder cb)
        {
            cb.RegisterControllers(typeof (HomeController).Assembly);
            cb.RegisterModelBinders(typeof (HomeController).Assembly);
            cb.RegisterModelBinderProvider();
        }
    }
}