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

namespace Alphacloud.Common.Core.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using Data;
    using JetBrains.Annotations;

    /// <summary>
    ///   Specifies key of resource to bind.
    /// </summary>
    /// <summary>
    ///   See <see cref="EnumResourceBinder" /> for details.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    [BaseTypeRequired(typeof (Enum))]
    public sealed class ResourceBindingAttribute : Attribute
    {
        /// <summary>
        ///   Resource key.
        /// </summary>
        public string Key { get; set; }
    }


    /// <summary>
    ///   Key generation strategy.
    /// </summary>
    public enum KeyGenerationStategy
    {
        /// <summary>
        ///   Use numeric value.
        /// </summary>
        NumericValue,

        /// <summary>
        ///   Use field name.
        /// </summary>
        FieldName
    }


    /// <summary>
    ///   Load strings from resource.
    /// </summary>
    [PublicAPI]
    public static class EnumResourceBinder
    {
        /// <summary>
        ///   Create blank item.
        /// </summary>
        /// <returns></returns>
        public static SelectionItem BlankItem()
        {
            return new SelectionItem {Selected = true, Text = string.Empty, Value = string.Empty};
        }


        /// <summary>
        ///   Get string from resource.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <param name="resourceManager">Resource manager instance.</param>
        /// <param name="enum">Enum.</param>
        /// <returns>String from resource or enum member name if string does not exist.</returns>
        [NotNull]
        public static string GetString<T>(ResourceManager resourceManager, T @enum)
            //where T: enum
        {
            CheckIfTypeIsEnum<T>();
            var type = @enum.GetType();
            var memberInfo = type.GetMember(@enum.ToString());
            if (memberInfo.Length != 1)
                return @enum.ToString();
            return GetText<T>(resourceManager, memberInfo[0]) ?? memberInfo[0].Name;
        }


        /// <summary>
        ///   Loads strings from resource based on enum members.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="resourceManager">The resourceManager.</param>
        /// <param name="keyGenerationStategy">Should key values be generated based on ordinal value or string representation of member.</param>
        /// <returns>Mapping Enum field=Resource string</returns>
        [NotNull]
        //// ReSharper disable ReturnTypeCanBeEnumerable.Global
        public static IDictionary<string, string> LoadStrings<T>(ResourceManager resourceManager,
            KeyGenerationStategy keyGenerationStategy)
            //// ReSharper restore ReturnTypeCanBeEnumerable.Global
            //where T: Enum
        {
            var keyGenerator = CreateKeyGenerator(keyGenerationStategy);

            CheckIfTypeIsEnum<T>();
            var data = new Dictionary<string, string>();
            foreach (
                var fieldInfo in typeof (T).GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public)
                )
            {
                var str = GetText<T>(resourceManager, fieldInfo);
                if (str != null)
                    data[keyGenerator(fieldInfo)] = str;
            }
            return data;
        }


        private static Func<FieldInfo, string> CreateKeyGenerator(KeyGenerationStategy keyGenerationStategy)
        {
            Func<FieldInfo, string> keyGenerator;
            switch (keyGenerationStategy)
            {
                case KeyGenerationStategy.NumericValue:
                    keyGenerator = fi => (Convert.ToInt32(fi.GetValue(null))).ToString(CultureInfo.InvariantCulture);
                    break;
                case KeyGenerationStategy.FieldName:
                    keyGenerator = fi => fi.Name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("keyGenerationStategy");
            }
            return keyGenerator;
        }


        private static void CheckIfTypeIsEnum<T>()
        {
            if (!typeof (T).IsEnum)
                throw new InvalidOperationException("Type {0} is not enum".ApplyArgs(typeof (T)));
        }


        private static string GetText<T>(ResourceManager resourceManager, MemberInfo fieldInfo)
        {
            var attrs = fieldInfo.GetCustomAttributes(typeof (ResourceBindingAttribute), false);
            if (attrs.Length == 1)
            {
                var key = GetResourceKey(fieldInfo, (ResourceBindingAttribute) attrs[0]);
                var str = GetResouce(resourceManager, key) ?? string.Empty;
                return str;
            }
            return null;
        }


        private static string GetResouce(ResourceManager resources, string key)
        {
            return resources.GetString(key);
        }


        internal static string GetResourceKey(MemberInfo fieldInfo, ResourceBindingAttribute attr)
        {
            var key = attr.Key ?? "{0}_{1}".ApplyArgs(fieldInfo.DeclaringType.Name, fieldInfo.Name);
            return key;
        }


        /// <summary>
        ///   Generate selection list for enum.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <param name="resourceManager">Resource manager.</param>
        /// <param name="keyGenerationStategy">
        ///   <see cref="KeyGenerationStategy" />
        /// </param>
        /// <param name="selector">Action to set selected item.</param>
        /// <param name="insertBlankItem">Should blank item be inserted?</param>
        /// <returns>Selection list</returns>
        public static IEnumerable<SelectionItem> SelectionListFor<T>(ResourceManager resourceManager,
            KeyGenerationStategy keyGenerationStategy = KeyGenerationStategy.FieldName,
            Func<string, bool> selector = null, bool insertBlankItem = false)
        {
            var data = LoadStrings<T>(resourceManager, keyGenerationStategy);
            var checkSelected = selector ?? (s => false);
            var list = data.Select(
                kvp =>
                    new SelectionItem
                    {
                        Value = kvp.Key,
                        Text = kvp.Value ?? kvp.Key,
                        Selected = checkSelected(kvp.Key)
                    })
                .ToList();
            if (insertBlankItem)
                list.Insert(0, BlankItem());
            return list;
        }
    }

    /// <summary>
    ///   Selection list.
    /// </summary>
    [Serializable]
    public class SelectionItem
    {
        /// <summary>
        ///   Display text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///   Item value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///   Is item selected?
        /// </summary>
        public bool Selected { get; set; }
    }
}