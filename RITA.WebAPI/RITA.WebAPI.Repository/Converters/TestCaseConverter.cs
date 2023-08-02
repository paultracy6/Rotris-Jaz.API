using RITA.EF.Models;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Repository.Models;

namespace RITA.WebAPI.Repository.Converters;

internal static class TestCaseConverter
{
    public static TestCase ToTestCase(this ITestCaseModel testCaseModel)
    {
        var testCase = new TestCase
        {
            Id = testCaseModel.Id,
            Name = testCaseModel.Name,
            SuiteId = testCaseModel.SuiteId,
            Url = testCaseModel.Url,
            CreatedBy = testCaseModel.CreatedBy,
            CreatedOn = testCaseModel.CreatedOn,
            UpdatedBy = testCaseModel.UpdatedBy,
            UpdatedOn = testCaseModel.UpdatedOn
        };

        return testCase;
    }

    public static ITestCaseModel ToITestCaseModel(this TestCase testCase)
    {
        var testCaseModel = new TestCaseModel()
        {
            Id = testCase.Id,
            Name = testCase.Name,
            SuiteId = testCase.SuiteId,
            Url = testCase.Url,
            CreatedBy = testCase.CreatedBy,
            CreatedOn = testCase.CreatedOn,
            UpdatedBy = testCase.UpdatedBy,
            UpdatedOn = testCase.UpdatedOn
        };

        return testCaseModel;
    }

    public static IEnumerable<ITestCaseModel> ToITestCaseModelEnumerable(this IQueryable<TestCase> testCases)
    {
        var models = new List<ITestCaseModel>();
        foreach (TestCase testCase in testCases)
        {
            models.Add(testCase.ToITestCaseModel());
        }

        return models;
    }
}