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

namespace Alphacloud.Common.Core.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using JetBrains.Annotations;

    [PublicAPI]
    public static class TypeExtensions
    {
        /// <summary>
        ///   Check if <paramref name="thisType" /> implements open generic type <paramref name="genericInterface" />
        /// </summary>
        /// <param name="thisType"></param>
        /// <param name="genericInterface"></param>
        /// <returns></returns>
        public static bool ImplementsGeneric([NotNull] this Type thisType, [NotNull] Type genericInterface)
        {
            if (thisType == null) throw new ArgumentNullException(nameof(thisType));
            if (genericInterface == null) throw new ArgumentNullException(nameof(genericInterface));
            if (!genericInterface.IsGenericTypeDefinition)
                throw new ArgumentException("Type not represents generic type definition", nameof(genericInterface));
            if (!genericInterface.IsInterface)
                throw new ArgumentException("Argument is not Interface", nameof(genericInterface));

            return thisType.GetInterfaces().Where(intf => intf.IsGenericType)
                .Any(intf => intf.GetGenericTypeDefinition() == genericInterface);
        }

        /// <summary>
        /// Find non-generic interfaces inherited from <paramref name="genericInterface"/> implemented by <paramref name="thisType"/>.
        /// </summary>
        /// <param name="thisType"></param>
        /// <param name="genericInterface">Generic interface type.</param>
        /// <returns>Interface types definitions.</returns>
        public static IEnumerable<Type> FindNonGenericInterfacesOf([NotNull] this Type thisType, [NotNull] Type genericInterface)
        {
            if (genericInterface == null) throw new ArgumentNullException(nameof(genericInterface));
            if (thisType == null) throw new ArgumentNullException(nameof(thisType));

            if (!genericInterface.IsInterface || !genericInterface.IsGenericType ||
                !genericInterface.ContainsGenericParameters)
                throw new ArgumentException(
                    "Type '{0}' is not open generic interface".ApplyArgs(genericInterface.FullName));

            return thisType.FindInterfaces((type, o) =>
                !type.IsGenericType && type.ImplementsGeneric(genericInterface), null);
        }


    }
}