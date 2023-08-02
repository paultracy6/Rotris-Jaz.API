using RITA.EF.Models;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Repository.Models;

namespace RITA.WebAPI.Repository.Converters
{
    internal static class SuiteConverter
    {
        public static Suite ToSuite(this ISuiteModel suiteModel)
        {
            var suite = new Suite
            {
                Id = suiteModel.Id,
                Name = suiteModel.Name,
                AppId = suiteModel.AppId,
                CreatedBy = suiteModel.CreatedBy,
                CreatedOn = suiteModel.CreatedOn,
                UpdatedBy = suiteModel.UpdatedBy,
                UpdatedOn = suiteModel.UpdatedOn
            };

            return suite;
        }

        public static ISuiteModel ToISuiteModel(this Suite suite)
        {
            var suiteModel = new SuiteModel
            {
                Id = suite.Id,
                Name = suite.Name,
                AppId = suite.AppId,
                CreatedBy = suite.CreatedBy,
                CreatedOn = suite.CreatedOn,
                UpdatedBy = suite.UpdatedBy,
                UpdatedOn = suite.UpdatedOn
            };

            return suiteModel;
        }

        public static IEnumerable<ISuiteModel> ToISuiteModelEnumerable(this IQueryable<Suite> suites)
        {
            var models = new List<ISuiteModel>();
            foreach (Suite suite in suites)
            {
                models.Add(suite.ToISuiteModel());
            }

            return models;
        }
    }
}
