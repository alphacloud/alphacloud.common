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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Data;
    using JetBrains.Annotations;

    /// <summary>
    ///   Reflection helpers.
    /// </summary>
    [PublicAPI]
    public static class ReflectionExtensions
    {
        static readonly Type[] s_noTypes = new Type[0];
        static readonly ParameterModifier[] s_noParams = new ParameterModifier[0];


        /// <summary>
        ///   Collect public property values to dictionary.
        /// </summary>
        /// <param name="obj">Source.</param>
        /// <returns>Propery values as dictionary.</returns>
        /// <remarks>
        ///   Only public readable non-indexes properties are processed.
        /// </remarks>
        public static IDictionary<string, object> ToDataDictionary([NotNull] this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var props = from p in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                where p.CanRead && !p.GetIndexParameters().Any()
                select new KeyValuePair<string, object>(p.Name, p.GetValue(obj, null));

            return props.ToDictionary(k => k.Key, v => v.Value);
        }


        /// <summary>
        ///   Returns instance property value.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="obj">Object to retrieve proprty from.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Value</returns>
        /// <exception cref="NullReferenceException"><paramref name="obj" /> is <c>null</c></exception>
        /// <exception cref="ArgumentException">Property does not exist.</exception>
        public static T PropertyValue<T>([NotNull] this object obj, string propertyName)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException(@"Property name not specified.", propertyName);
            var propInfo = obj.GetType()
                .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
                    typeof (T), s_noTypes, s_noParams);
            if (propInfo == null)
                throw new ArgumentException("Invalid property name '{0}'.".ApplyArgs(propertyName), "propertyName");


            return (T) propInfo.GetValue(obj, null);
        }


        /// <summary>
        ///   Returns field value.
        /// </summary>
        /// <typeparam name="T">Field type.</typeparam>
        /// <param name="obj">Object to retrieve field from.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Field value.</returns>
        /// <exception cref="NullReferenceException"><paramref name="obj" /> is <c>null</c></exception>
        /// <exception cref="ArgumentException">Property does not exist.</exception>
        /// <exception cref="InvalidCastException">Value can not be casted to expected type.</exception>
        public static T FieldValue<T>([NotNull] this object obj, string fieldName)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentException(@"Field name not specified.", fieldName);

            var fieldInfo = obj.GetType()
                .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (fieldInfo == null)
                throw new ArgumentException("Invalid field name '{0}'.".ApplyArgs(fieldName), "fieldName");

            return (T) fieldInfo.GetValue(obj);
        }
    }
}