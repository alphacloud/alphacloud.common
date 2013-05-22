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
    using Microsoft.Practices.ServiceLocation;
    using global::Castle.MicroKernel;

    /// <summary>
    /// CommonServiceLocator adapter for Castle MicroKernel
    /// </summary>
    public class MicroKernelServiceLocatorAdapter : ServiceLocatorImplBase
    {
        private readonly IKernel _kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicroKernelServiceLocatorAdapter"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <exception cref="System.ArgumentNullException">kernel</exception>
        public MicroKernelServiceLocatorAdapter(IKernel kernel)
        {
            if (kernel == null) throw new ArgumentNullException("kernel");
            _kernel = kernel;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return string.IsNullOrEmpty(key)
                ? _kernel.Resolve(serviceType)
                : _kernel.Resolve(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return _kernel.ResolveAll(serviceType).OfType<object>();
        }
    }
}