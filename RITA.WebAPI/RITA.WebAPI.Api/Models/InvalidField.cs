﻿using RITA.WebAPI.Abstractions.Validation;

namespace RITA.WebAPI.Api.Models
{
    public class InvalidField : IInvalidField
    {
        public string Name { get; set; }
        public string Message { get; set; }

        public InvalidField() { }
        public InvalidField(string name, string message)
        {
            Name = name;
            Message = message;
        }
    }
}
