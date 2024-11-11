using System.ComponentModel;
using System.Reflection;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            Type type = value.GetType();
            string name = Enum.GetName(type, value)!;

            if (name != null)
            {
                FieldInfo field = type.GetField(name)!;

                if (field != null)
                {
                    var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

                    DescriptionAttribute? descAttr = null;

                    if (attribute != null)
                    {
                        descAttr = (attribute as DescriptionAttribute)!;
                    }

                    if (attribute != null)
                    {
                        return descAttr!.Description;
                    }
                }
            }

            return string.Empty;
        }
    }
}
