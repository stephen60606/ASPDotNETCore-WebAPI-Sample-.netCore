using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NetCore.Extensions
{
    /// <summary>
    /// extension methods for enum display and convert
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// get enum DisplayAttribute Name, otherwise return its value
        /// </summary>
        /// <param name="enumValue">Enum</param>
        /// <returns>Display Name(</returns>
        public static string ToDisplayName(this Enum enumValue)
        {
            var enumMember = enumValue.GetType().GetMember(enumValue.ToString());

            DisplayAttribute displayAttribute = null;
            if (enumMember.Any())
            {
                displayAttribute = enumMember.First().GetCustomAttribute<DisplayAttribute>();
            }

            string displayName = displayAttribute?.GetName();

            return displayName ?? enumValue.ToString();
        }

        /// <summary>
        /// convert enum value to string
        /// </summary>
        /// <param name="enumValue">Enum</param>
        /// <returns>Value String</returns>
        public static string ToValueString(this Enum enumValue)
        {
            return enumValue.ToString("D");
        }

        /// <summary>
        /// convert string to Enum
        /// </summary>
        /// <typeparam name="T">Generic</typeparam>
        /// <param name="enumAsString">String</param>
        /// <returns>result after converted</returns>
        public static T ToEnum<T>(this string enumAsString)
        {
            return (T)Enum.Parse(typeof(T), enumAsString, true);
        }

        /// <summary>
        /// convert displayName string to Enum
        /// </summary>
        /// <typeparam name="T">Generic</typeparam>
        /// <param name="displayName">String</param>
        /// <param name="defaultVal">default value</param>
        /// <returns>result after converted</returns>
        public static T ToEnumFromDisplayName<T>(this string displayName, T defaultVal)
        {
            var type = typeof(T);
            if (!type.IsEnum) return defaultVal;

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DisplayAttribute)) as DisplayAttribute;
                if (attribute != null)
                {
                    if (attribute.Name == displayName)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == displayName)
                        return (T)field.GetValue(null);
                }
            }

            return defaultVal;
        }
    }
}

