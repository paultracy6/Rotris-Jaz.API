namespace RITA.WebAPI.Abstractions.Validation;

public interface IInvalidField
{
    string Name { get; set; }
    string Message { get; set; }
}