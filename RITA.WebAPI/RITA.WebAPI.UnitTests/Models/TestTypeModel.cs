﻿using RITA.WebAPI.Abstractions.Models;

namespace RITA.WebAPI.UnitTests.Models;

public class TestTypeModel : ITestTypeModel
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    public string Name { get; set; } = string.Empty;
}