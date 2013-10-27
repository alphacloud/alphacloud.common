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

namespace Alphacloud.Common.ServiceLocator.Castle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Castle.Windsor;
    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    ///   CommonServiceLocator adapter for Castle Windsor
    /// </summary>
    public sealed class WindsorServiceLocatorAdapter : ServiceLocatorImplBase
    {
        readonly IWindsorContainer _container;


        /// <summary>
        ///   Initializes a new instance of the <see cref="WindsorServiceLocatorAdapter" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentNullException">container</exception>
        public WindsorServiceLocatorAdapter(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _container = container;
        }


        /// <summary>
        ///   When implemented by inheriting classes, this method will do the actual work of resolving
        ///   the requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>
        ///   The requested service instance.
        /// </returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            return string.IsNullOrEmpty(key)
                ? _container.Resolve(serviceType)
                : _container.Resolve(key, serviceType);
        }


        /// <summary>
        ///   When implemented by inheriting classes, this method will do the actual work of
        ///   resolving all the requested service instances.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>
        ///   Sequence of service instance objects.
        /// </returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return _container.ResolveAll(serviceType).OfType<object>();
        }
    }
}