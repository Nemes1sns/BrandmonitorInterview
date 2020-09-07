using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Helpers
{
    public static class EnumHelper
    {
        public static string DisplayName(this Enum value)
        {
            var enumType = value.GetType();
            string enumName = Enum.GetName(enumType, value);
            var displayAttribute = enumType.GetMember(enumName).Single().GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.GetName() ?? value.ToString();
        }
    }
}