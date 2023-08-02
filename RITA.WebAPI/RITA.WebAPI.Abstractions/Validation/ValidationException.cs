namespace RITA.WebAPI.Abstractions.Validation
{
    public class ValidationException : Exception, IValidationException
    {
        public IList<IInvalidField> InvalidFields;

        public ValidationException(string message, IList<IInvalidField> invalidFields) : base(message)
        {
            InvalidFields = invalidFields;
        }

        public string GetMessage()
        {
            List<string> errors = new List<string>();
            InvalidFields.ToList().ForEach(field =>
            {
                errors.Add(field.Name + "-" + field.Message);
            }
            );
            return Message + ": " + string.Join(',', errors);
        }
    }
}