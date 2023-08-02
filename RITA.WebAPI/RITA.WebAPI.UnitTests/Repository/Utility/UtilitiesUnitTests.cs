using NUnit.Framework;
using RITA.EF.Models;
using System.ComponentModel.DataAnnotations;
using static RITA.WebAPI.Repository.Utility.ValidationUtility;

namespace RITA.WebAPI.UnitTests.Repository.Utility;

public class UtilitiesUnitTests
{
    [TestCase]
    public void ValidationUtility_FailInvalidPropertyName()
    {
        var suite = new Suite();
        var maxLength = suite.GetAttributeFrom<MaxLengthAttribute>("Poopy Head");

        var ex = Assert.Throws<Exception>(() => CheckMaxLengthValid(suite, "Poopy Head"));
        Assert.AreEqual("Property Not Found - Poopy Head", ex.Message);
    }
}