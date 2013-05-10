namespace Alphacloud.Common.Core.Resources
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Resources;

    using Alphacloud.Common.Core.Data;

    using JetBrains.Annotations;

    #endregion

    /// <summary>
    ///     Specified key of resource to bind.
    /// </summary>
    /// <summary>
    ///     See <see cref="EnumResourceBinder" /> for details.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    [BaseTypeRequired(typeof (Enum))]
    public sealed class ResourceBindingAttribute : Attribute
    {
        public string Key { get; set; }
    }


    /// <summary>
    ///     Key generation strategy.
    /// </summary>
    public enum KeyGenerationStategy
    {
        /// <summary>
        ///     Use numeric value
        /// </summary>
        NumericValue,

        /// <summary>
        ///     Use field name.
        /// </summary>
        FieldName
    }


    /// <summary>
    ///     Load strings from resource.
    /// </summary>
    [PublicAPI]
    public static class EnumResourceBinder
    {
        public static SelectionItem BlankItem()
        {
            return new SelectionItem { Selected = true, Text = string.Empty, Value = string.Empty };
        }


        public static string GetString <T>(ResourceManager resourceManager, T @enum)
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
        ///     Loads strings from resource based on enum members.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="resourceManager">The resourceManager.</param>
        /// <param name="keyGenerationStategy">Should key values be generated based on ordinal value or string representation of member.</param>
        /// <returns>Mapping Enum field=Resource string</returns>
        [NotNull]
        //// ReSharper disable ReturnTypeCanBeEnumerable.Global
        public static IDictionary<string, string> LoadStrings <T>(ResourceManager resourceManager,
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


        static Func<FieldInfo, string> CreateKeyGenerator(KeyGenerationStategy keyGenerationStategy)
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


        static void CheckIfTypeIsEnum <T>()
        {
            if (!typeof (T).IsEnum)
                throw new InvalidOperationException("Type {0} is not enum".ApplyArgs(typeof (T)));
        }


        static string GetText <T>(ResourceManager resourceManager, MemberInfo fieldInfo)
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


        static string GetResouce(ResourceManager resources, string key)
        {
            return resources.GetString(key);
        }


        internal static string GetResourceKey(MemberInfo fieldInfo, ResourceBindingAttribute attr)
        {
            var key = attr.Key ?? "{0}_{1}".ApplyArgs(fieldInfo.DeclaringType.Name, fieldInfo.Name);
            return key;
        }


        public static IEnumerable<SelectionItem> SelectionListFor <T>(ResourceManager resourceManager,
            KeyGenerationStategy keyGenerationStategy = KeyGenerationStategy.FieldName,
            Func<string, bool> selector = null, bool insertBlankItem = false)
        {
            var data = LoadStrings<T>(resourceManager, keyGenerationStategy);
            var checkSelected = selector ?? (s => false);
            var list = data.Select(
                kvp =>
                    new SelectionItem {
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

    [Serializable]
    public class SelectionItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
}
