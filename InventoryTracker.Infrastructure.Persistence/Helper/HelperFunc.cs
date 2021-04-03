using InventoryTracker.Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InventoryTracker.Infrastructure.Persistence
{
    public class HelperFunc
    {
        public static T CopyProps<T>(T src, T dest)
        {
            if (src == null)
                return default;

            foreach (PropertyInfo prop in src.GetType().GetProperties())
            {
                if (prop.CanWrite && prop.GetCustomAttribute(typeof(CopyIgnoreAttribute)) == null &&
                    (prop.PropertyType.IsValueType || prop.PropertyType == typeof(string)))
                {
                    object value = prop.GetValue(src);
                    prop.SetValue(dest, value);
                }
            }

            return dest;
        }
    }
}
