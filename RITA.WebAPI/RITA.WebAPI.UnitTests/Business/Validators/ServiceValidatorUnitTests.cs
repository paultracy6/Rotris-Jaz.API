using NUnit.Framework;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Business.Validators;
using RITA.WebAPI.UnitTests.Common;
using RITA.WebAPI.UnitTests.Models;
using System.Diagnostics.CodeAnalysis;

namespace RITA.WebAPI.UnitTests.Business.Validators;

[ExcludeFromCodeCoverage]
public class ServiceValidatorUnitTests
{
    private ICommonModel _insertCommonModel;
    private ICommonModel _updateCommonModel;
    private readonly IServiceValidator _serviceValidator;

    public ServiceValidatorUnitTests()
    {
        _serviceValidator = new ServiceValidator();
    }

    [SetUp]
    public void TestSetup()
    {
        _insertCommonModel = new CommonModel()
        {
            CreatedBy = "Jazz",
            CreatedOn = DateTime.Today,
            Id = 0
        };
        _updateCommonModel = new CommonModel()
        {
            UpdatedBy = "JazzUpdate",
            UpdatedOn = DateTime.Today,
            Id = 7
        };
    }

    [TestCase]
    public void ValidateInsert_Id_Not_0_Failure()
    {
        var field = new InvalidField("Id", "must be zero");
        _insertCommonModel.Id = 3;

        var result = _serviceValidator.ValidateInsert(_insertCommonModel);
        CommonValidation.ValidateActualErrorList(result, field);
    }

    [TestCase]
    public void ValidateInsert_CreatedOn_NotToday_Failure()
    {
        var field = new InvalidField("CreatedOn", "must be current date");
        _insertCommonModel.CreatedOn = new DateTime(2023, 1, 1);

        var result = _serviceValidator.ValidateInsert(_insertCommonModel);
        CommonValidation.ValidateActualErrorList(result, field);
    }

    [TestCase]
    public void ValidateInsert_CreatedOn_MinValue()
    {
        var field = new InvalidField("CreatedOn", "must be current date");
        _insertCommonModel.CreatedOn = DateTime.MinValue;

        var result = _serviceValidator.ValidateInsert(_insertCommonModel);
        CommonValidation.ValidateActualErrorList(result, field);
    }

    [TestCase]
    public void ValidateInsert_CreatedBy_EmptyString_Failure()
    {
        var field = new InvalidField("CreatedBy", "cannot be empty on insert");
        _insertCommonModel.CreatedBy = string.Empty;

        var result = _serviceValidator.ValidateInsert(_insertCommonModel);
        CommonValidation.ValidateActualErrorList(result, field);
    }

    [TestCase]
    public void ValidateInsert_UpdatedOn_NotEmpty_Failure()
    {
        var field = new InvalidField("UpdatedOn", "must be empty");
        _insertCommonModel.UpdatedOn = DateTime.Today;

        var result = _serviceValidator.ValidateInsert(_insertCommonModel);
        CommonValidation.ValidateActualErrorList(result, field);
    }

    [TestCase]
    public void ValidateInsert_UpdatedBy_NotEmpty_Failure()
    {
        var field = new InvalidField("UpdatedBy", "must be empty");
        _insertCommonModel.UpdatedBy = "Jazz";

        var result = _serviceValidator.ValidateInsert(_insertCommonModel);
        CommonValidation.ValidateActualErrorList(result, field);
    }

    [TestCase]
    public void ValidateUpdate_Id_Not_GreaterThanZero_Failure()
    {
        var field = new InvalidField("Id", "must be greater than zero");
        _updateCommonModel.Id = -7;

        var result = _serviceValidator.ValidateUpdate(_updateCommonModel);
        CommonValidation.ValidateActualErrorList(result, field);
    }

    [TestCase]
    public void ValidateUpdate_UpdatedOn_NotToday_Failure()
    {
        var field = new InvalidField("UpdatedOn", "must be current date");
        _updateCommonModel.UpdatedOn = new DateTime(2023, 2, 2);

        var result = _serviceValidator.ValidateUpdate(_updateCommonModel);
        CommonValidation.ValidateActualErrorList(result, field);
    }

    [TestCase]
    public void ValidateUpdate_UpdatedBy_EmptyString_Failure()
    {
        var field = new InvalidField("UpdatedBy", "cannot be empty on update");
        _updateCommonModel.UpdatedBy = string.Empty;

        var result = _serviceValidator.ValidateUpdate(_updateCommonModel);
        CommonValidation.ValidateActualErrorList(result, field);
    }
}