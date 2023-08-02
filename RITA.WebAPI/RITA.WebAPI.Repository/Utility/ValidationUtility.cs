using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RITA.WebAPI.Repository.Utility;

public static class ValidationUtility
{
    public static (bool isValid, int? maxLength) CheckMaxLengthValid(object instance, string propertyName)
    {
        var maxLength = instance.GetAttributeFrom<MaxLengthAttribute>(propertyName);
        PropertyInfo propertyInfo = instance.GetType().GetProperty(propertyName);
        var propertyValue = propertyInfo?.GetValue(instance);

        if (propertyValue == null) throw new Exception($"Property Not Found - {propertyName}");

        var stringLength = propertyValue.ToString()!.Length;
        return (stringLength <= maxLength.Length, maxLength.Length);
    }

    public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
    {
        Type attrType = typeof(T);
        PropertyInfo property = instance.GetType().GetProperty(propertyName);
        if (property != null) return (T)property.GetCustomAttributes(attrType, false).First();
        return null;
    }
}