using RITA.EF.Models;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Repository.Models;
using System.Reflection;

namespace RITA.WebAPI.Repository.Converters;

public static class TestDataConverter
{
    public static TestData ToTestData(this ITestDataModel testDataModel)
    {
        var testData = new TestData
        {
            Suspended = testDataModel.IsSuspended
        };

        PropertyInfo[] propertyInfo = testData.GetType().GetProperties();
        foreach (PropertyInfo property in propertyInfo)
        {
            if (property.Name == "Suspended") continue;

            var tdm = testDataModel.GetType().GetProperty(property.Name);
            if (tdm != null)
                property.SetValue(testData, tdm.GetValue(testDataModel));
        }

        return testData;
    }

    public static ITestDataModel ToITestDataModel(this TestData testData)
    {
        var testDataModel = new TestDataModel()
        {
            IsSuspended = testData.Suspended
        };

        PropertyInfo[] propertyInfo = testDataModel.GetType().GetProperties();
        foreach (PropertyInfo property in propertyInfo)
        {
            if (property.Name == "IsSuspended") continue;

            var td = testData.GetType().GetProperty(property.Name);
            if (td != null)
                property.SetValue(testDataModel, td.GetValue(testData));
        }

        return testDataModel;
    }

    public static IEnumerable<ITestDataModel> ToITestDataModelEnumerable(this IQueryable<TestData> testDatas)
    {
        var models = new List<ITestDataModel>();
        foreach (TestData testData in testDatas)
        {
            models.Add(testData.ToITestDataModel());
        }

        return models;
    }
}