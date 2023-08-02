using NUnit.Framework;
using RITA.WebAPI.Abstractions.Validation;
using System.Diagnostics.CodeAnalysis;

namespace RITA.WebAPI.UnitTests.Common;
[ExcludeFromCodeCoverage]
public static class CommonValidation
{
    internal static void InsertTest<T>(IInvalidField field, IValidator<T> validator, T model, string expectedMessageTitle)
    {
        ValidationException ex = Assert.Throws<ValidationException>(() => validator.ValidateInsert(model));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(expectedMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    internal static void UpdateTest<T>(IInvalidField field, IValidator<T> validator, T model,
        string expectedMessageTitle)
    {
        ValidationException ex = Assert.Throws<ValidationException>(() => validator.ValidateUpdate(model));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(expectedMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    internal static void ValidateActualErrorList(IList<IInvalidField> result, IInvalidField field)
    {
        Assert.NotNull(result);
        var actual = result.ToList();

        Assert.Multiple(() =>
        {
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(field.Name, actual[0].Name);
            Assert.AreEqual(field.Message, actual[0].Message);
        });

    }
}