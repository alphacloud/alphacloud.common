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
