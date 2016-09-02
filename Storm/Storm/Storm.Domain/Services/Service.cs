using DomainValidation.Validation;
using MongoDB.Bson;
using Storm.Domain.Interfaces.Service;
using System.Collections.Generic;

namespace Storm.Domain.Services
{
    public class Service : IService
    {
        public ValidationResult ValidateModel<T>(T model, Dictionary<string, string> keyValue)
        {
            var modelSerialized = model.ToBsonDocument();
            var validationResult = new ValidationResult();
            
            foreach (var item in keyValue)
            {
                BsonElement value;
                if (!modelSerialized.TryGetElement(item.Key, out value))
                    validationResult.Add(new ValidationError("Attribute " + item.Key + " does not exist"));
            }

            return validationResult;
        }
    }
}
