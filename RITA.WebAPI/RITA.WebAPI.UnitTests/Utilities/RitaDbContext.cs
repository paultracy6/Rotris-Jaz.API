using Microsoft.EntityFrameworkCore;
using RITA.EF;
using RITA.EF.Models;

namespace RITA.WebAPI.UnitTests.Utilities;

public static class RitaDbContext
{
    public static RitaContext? Context { get; set; }

    public static void BuildData()
    {
        if (Context == null)
        {
            var options = CreateInMemoryDbContextOptions("RitaDb");
            Context = new RitaContext(options);
            Context = AddSuiteData(Context);
            Context = AddTestCaseData(Context);
            Context = AddTestTypeData(Context);
            Context = AddContentTypeData(Context);
            Context = AddTestData_Data(Context);
            Context = AddFavoriteData(Context);
        }
    }

    private static DbContextOptions<RitaContext> CreateInMemoryDbContextOptions(string dbName)
    {
        return new DbContextOptionsBuilder<RitaContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
    }

    private static RitaContext AddSuiteData(RitaContext context)
    {
        context.Suites.Add(new Suite { Id = 1, AppId = 10, Name = "Test", CreatedBy = "Me" });
        context.Suites.Add(new Suite { Id = 2, AppId = 10, Name = "Test2", CreatedBy = "Me2" });
        context.Suites.Add(new Suite { Id = 3, AppId = 11, Name = "Test3", CreatedBy = "Me3" });
        context.SaveChanges();

        return context;
    }

    private static RitaContext AddTestCaseData(RitaContext context)
    {
        context.TestCases.Add(new TestCase
        {
            Id = 101,
            Name = "TestCase1",
            Url = "/test-cases/",
            RequestMethod = "Post",
            SuiteId = 1,
            CreatedBy = "Me"
        });
        context.TestCases.Add(new TestCase
        {
            Id = 102,
            Name = "TestCase2",
            Url = "/test-cases/",
            RequestMethod = "Delete",
            SuiteId = 1,
            CreatedBy = "Me"
        });
        context.TestCases.Add(new TestCase
        {
            Id = 103,
            Name = "TestCase3",
            Url = "/test-cases/",
            RequestMethod = "Get",
            SuiteId = 2,
            CreatedBy = "Me"
        });
        context.TestCases.Add(new TestCase
        {
            Id = 104,
            Name = "TestCase4",
            Url = "/test-cases/",
            RequestMethod = "Put",
            SuiteId = 3,
            CreatedBy = "Me"
        });
        context.SaveChanges();

        return context;
    }

    private static RitaContext AddTestTypeData(RitaContext context)
    {
        context.TestTypes.Add(new TestType
        {
            Id = 601,
            Name = "Integration",
            CreatedBy = "Me",
            CreatedOn = new DateTime(2023, 2, 22),
            UpdatedOn = new DateTime(2023, 2, 24),
            UpdatedBy = "Me2"
        });
        return context;
    }

    private static RitaContext AddContentTypeData(RitaContext context)
    {
        context.ContentTypes.Add(new ContentType
        {
            Id = 501,
            CreatedBy = "Me",
            CreatedOn = new DateTime(2023, 2, 22),
            MimeType = "application/json"
        });
        context.ContentTypes.Add(new ContentType
        {
            Id = 502,
            CreatedBy = "Me",
            CreatedOn = new DateTime(2023, 2, 22),
            MimeType = "text/plain"
        });
        return context;
    }

    private static RitaContext AddTestData_Data(RitaContext context)
    {
        context.TestData.Add(new TestData
        {
            Id = 201,
            Name = "TestData1",
            Suspended = true,
            SuspendedOn = new DateTime(2023, 2, 22),
            SuspendedBy = "Maidul",
            CreatedBy = "Me",
            CreatedOn = new DateTime(2023, 2, 22),
            Comment = string.Empty,
            TestCaseId = 102,
            TestTypeId = 601,
            TestCase = context.TestCases.Find(102),
            TestType = context.TestTypes.Find(601),
            StatusCode = 200,
            RequestContent = "{\"LoanIdentifier\": \"9999999999\" }",
            RequestContentTypeId = 501,
            ResponseContent = "{\"LoanIdentifier\": \"9999999999\" }",
            ResponseContentTypeId = 501
        });
        context.TestData.Add(new TestData
        {
            Id = 202,
            Name = "TestData2",
            Suspended = false,
            CreatedBy = "Me",
            CreatedOn = new DateTime(2023, 2, 22),
            TestCaseId = 102,
            TestTypeId = 601,
            TestCase = context.TestCases.Find(102),
            TestType = context.TestTypes.Find(601),
            StatusCode = 200,
            RequestContent = "{\"LoanIdentifier\": \"9999999999\" }",
            RequestContentTypeId = 501,
            ResponseContent = "{\"LoanIdentifier\": \"9999999999\" }",
            ResponseContentTypeId = 501
        });
        context.TestData.Add(new TestData
        {
            Id = 203,
            Name = "TestData3",
            Suspended = false,
            CreatedBy = "Me",
            CreatedOn = new DateTime(2023, 2, 22),
            TestCaseId = 103,
            TestTypeId = 601,
            TestCase = context.TestCases.Find(103),
            TestType = context.TestTypes.Find(601),
            StatusCode = 200,
            RequestContent = "{\"LoanIdentifier\": \"9999999999\" }",
            RequestContentTypeId = 501,
            ResponseContent = "{\"LoanIdentifier\": \"9999999999\" }",
            ResponseContentTypeId = 501
        });
        context.TestData.Add(new TestData
        {
            Id = 204,
            Name = "TestData4",
            Suspended = false,
            CreatedBy = "Me",
            CreatedOn = new DateTime(2023, 2, 22),
            TestCaseId = 104,
            TestTypeId = 601,
            TestCase = context.TestCases.Find(104),
            TestType = context.TestTypes.Find(601),
            StatusCode = 200,
            RequestContent = "{\"LoanIdentifier\": \"9999999999\" }",
            RequestContentTypeId = 501,
            ResponseContent = "{\"LoanIdentifier\": \"9999999999\" }",
            ResponseContentTypeId = 501
        });
        context.TestData.Add(new TestData
        {
            Id = 205,
            Name = "TestData4",
            Suspended = false,
            CreatedBy = "Me",
            CreatedOn = new DateTime(2023, 2, 22),
            TestCaseId = 104,
            TestTypeId = 601,
            TestCase = context.TestCases.Find(104),
            TestType = context.TestTypes.Find(601),
            StatusCode = 200,
            RequestContent = "{\"LoanIdentifier\": \"9999999999\" }",
            RequestContentTypeId = 501,
            ResponseContent = "{\"LoanIdentifier\": \"9999999999\" }",
            ResponseContentTypeId = 501
        });
        context.SaveChanges();

        return context;
    }

    private static RitaContext AddFavoriteData(RitaContext context)
    {
        context.Favorites.Add(new Favorite
        {
            Id = 601,
            UserId = "user1",
            Name = "favorite1",
            FavoriteType = "T1",
            ReferenceId = 11
        });
        context.Favorites.Add(new Favorite
        {
            Id = 602,
            UserId = "user1",
            Name = "favorite2",
            FavoriteType = "T2",
            ReferenceId = 12
        });
        context.SaveChanges();

        return context;
    }
}