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

namespace Alphacloud.Common.Core.Utils
{
    using JetBrains.Annotations;

    /// <summary>
    ///   Generic factory interface.
    ///   Used to inject services.
    /// </summary>
    /// <typeparam name="TServiceContract">service interface</typeparam>
    [PublicAPI]
    public interface IServiceFactory<out TServiceContract>
        where TServiceContract : class
    {
        /// <summary>
        ///   Creates new service instance.
        /// </summary>
        /// <returns>Service instance.</returns>
        TServiceContract CreateService();
    }
}