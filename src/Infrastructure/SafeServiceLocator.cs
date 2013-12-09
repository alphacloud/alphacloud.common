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

namespace Alphacloud.Common.Infrastructure
{
    using System;
    using Core.Data;
    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    ///   Common Service Locator wrapper providing better error messages.
    /// </summary>
    public static class SafeServiceLocator
    {
        /// <summary>
        ///   Resolve service.
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <returns>Service instance</returns>
        /// <exception cref="InvalidOperationException">Service locator was not initialized.</exception>
        /// <exception cref="ActivationException">Error while resolving requested service.</exception>
        public static TService Resolve<TService>()
        {
            try
            {
                return ServiceLocator.Current.GetInstance<TService>();
            }
            catch (NullReferenceException nre)
            {
                throw new InvalidOperationException(
                    "Error retrieving type {0}: Service locator has not been initialized. Make sure corresponding CSL adapter is set."
                        .ApplyArgs(typeof (TService)), nre);
            }
            catch (ActivationException ae)
            {
                throw new ActivationException(
                    "Requested type {0} cannot be resolved. Make sure type is registered in IOC.".ApplyArgs(
                        typeof (TService)), ae);
            }
        }
    }
}