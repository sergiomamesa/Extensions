using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class EnumHelper
    {

        public static T ToEnum<T>(this string enumString)
        {
            return (T)Enum.Parse(typeof(T), enumString);
        }

        public static T ToEnum<T>(this int enumId)
        {
            return (T)Enum.ToObject(typeof(T), enumId);
        }

        public static string GetName<T>(this Enum value)
        {
            return Enum.GetName(typeof(T), value);
        }

        public static T GetEnumID<T>(this Enum value, string newvalue)
        {
            T enumId = Activator.CreateInstance<T>();
            FieldInfo field = enumId.GetType().GetField("name");
            field.SetValue(enumId, newvalue);

            return enumId;
        }
    }
}
