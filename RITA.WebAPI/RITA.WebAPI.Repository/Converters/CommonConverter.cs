using RITA.EF.Models;
using RITA.WebAPI.Abstractions.Models;
using System.Reflection;

namespace RITA.WebAPI.Repository.Converters;

public static class CommonConverter
{
    public static CommonFields ToCommonField(this ICommonModel model)
    {
        var commonField = new CommonFields();
        PropertyInfo[] propertyInfo = commonField.GetType().GetProperties();
        foreach (PropertyInfo property in propertyInfo)
        {
            var rf = model.GetType().GetProperty((property.Name));
            if (rf != null)
                property.SetValue(commonField, rf.GetValue(model));
        }

        return commonField;
    }
}