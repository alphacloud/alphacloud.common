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

namespace Alphacloud.Common.Core.Data
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using JetBrains.Annotations;


    /// <summary>
    ///   Enum helper.
    /// </summary>
    [PublicAPI]
    public static class EnumHelper
    {
        static readonly ConcurrentDictionary<string, string> s_descriptionCache =
            new ConcurrentDictionary<string, string>(3, 256);


        /// <summary>
        ///   Retrieves custom text for enum memeber, based on <see cref="DescriptionAttribute" /> value.
        ///   If enumeration member is not decorated with <c>Description</c> attribute, it's name will be returned instead.
        ///   <code>
        /// [Description("Bright Pink")]
        /// BrightPink = 2,
        /// </code>
        /// </summary>
        /// <param name="en">The Enumeration.</param>
        /// <returns>A string representing the friendly name</returns>
        public static string GetDescription(Enum en)
        {
            var type = en.GetType();
            var member = en.ToString();
            var key = string.Concat(type.FullName, ".", member);
            return s_descriptionCache.GetOrAdd(key, k => GetMemberDescription(type, member));
        }


        internal static string GetMemberDescription(Type enumType, string member)
        {
            var memInfo = enumType.GetMember(member);
            if (!memInfo.IsNullOrEmpty())
            {
                var attrs = memInfo[0].GetCustomAttributes(typeof (DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute) attrs[0]).Description;
                }
            }
            return member;
        }
    }
}