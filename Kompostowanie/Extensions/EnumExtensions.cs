using System;
using System.ComponentModel;

namespace Kompostowanie.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum value)
        {
            return GetEnumDescription(value);
        }

        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}