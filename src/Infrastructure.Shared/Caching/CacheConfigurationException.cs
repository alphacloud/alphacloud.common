#region copyright

// Copyright 2013-2015 Alphacloud.Net
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

namespace Alphacloud.Common.Infrastructure.Caching
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;

    /// <summary>
    ///   Represents error occured while processing cache configuration settings.
    /// </summary>
    [Serializable]
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class CacheConfigurationException : CacheException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheConfigurationException"/> class.
        /// </summary>
        protected CacheConfigurationException()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CacheConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CacheConfigurationException(string message) : base(message)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CacheConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CacheConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CacheConfigurationException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected CacheConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}