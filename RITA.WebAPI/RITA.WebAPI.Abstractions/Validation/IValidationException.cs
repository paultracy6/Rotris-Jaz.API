using System.Runtime.Serialization;

namespace RITA.WebAPI.Abstractions.Validation
{
    public interface IValidationException : ISerializable
    {
        string GetMessage();
    }
}
