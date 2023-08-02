using RITA.EF.Models;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Repository.Models;
using System.Reflection;

namespace RITA.WebAPI.Repository.Converters;

public static class TestTypeConverter
{
    public static ITestTypeModel ToITestTypeModel(this TestType testType)
    {
        var testTypeModel = new TestTypeModel();
        PropertyInfo[] propertyInfo = testTypeModel.GetType().GetProperties();
        foreach (PropertyInfo property in propertyInfo)
        {
            var td = testType.GetType().GetProperty(property.Name);
            if (td != null)
                property.SetValue(testTypeModel, td.GetValue(testType));
        }

        return testTypeModel;
    }

    public static IEnumerable<ITestTypeModel> ToITestTypeModelEnumerable(this IQueryable<TestType> testTypes)
    {
        var models = new List<ITestTypeModel>();

        if (testTypes == null) return models;

        foreach (TestType testType in testTypes)
        {
            models.Add(testType.ToITestTypeModel());
        }

        return models;
    }
}