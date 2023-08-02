using System.Reflection;

namespace RITA.WebAPI.Core.Enums;

public class StringValueAttribute : Attribute
{
    public string StringValue { get; protected set; }

    public StringValueAttribute(string value)
    {
        StringValue = value;
    }
}

public static class StringValueExtension
{
    public static string GetStringValue(this Enum value)
    {
        Type type = value.GetType();

        FieldInfo? fieldInfo = type.GetField(value.ToString());

        StringValueAttribute[]? attributes = fieldInfo?.GetCustomAttributes(
            typeof(StringValueAttribute), false) as StringValueAttribute[];

        return attributes != null && attributes.Length > 0 ? attributes[0].StringValue : value.ToString();
    }
}