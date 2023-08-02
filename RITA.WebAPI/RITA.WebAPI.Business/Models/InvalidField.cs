using RITA.WebAPI.Abstractions.Validation;

namespace RITA.WebAPI.Business.Models
{
    public class InvalidField : IInvalidField
    {
        public string Name { get; set; }
        public string Message { get; set; }

        public InvalidField()
        {
            Name = string.Empty;
            Message = string.Empty;
        }

        public InvalidField(string name, string message)
        {
            Name = name;
            Message = message;
        }
    }
}
