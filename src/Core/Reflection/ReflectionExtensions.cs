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

namespace Alphacloud.Common.Core.Reflection
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using JetBrains.Annotations;

    #endregion

    [PublicAPI]
    public static class ReflectionExtensions
    {
        public static IDictionary<string, object> ToDataDictionary([NotNull] this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var props = from p in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                where p.CanRead && !p.GetIndexParameters().Any()
                select new KeyValuePair<string, object>(p.Name, p.GetValue(obj, null));

            return props.ToDictionary(k => k.Key, v => v.Value);
        }
    }
}
