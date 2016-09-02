using DomainValidation.Validation;
using System.Collections.Generic;

namespace Storm.Domain.Interfaces.Service
{
    public interface IService
    {
        ValidationResult ValidateModel<T>(T model, Dictionary<string, string> keyValue);
    }
}
