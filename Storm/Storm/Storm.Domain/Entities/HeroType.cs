using DomainValidation.Validation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Storm.Domain.Validations.HeroType;

namespace Storm.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class HeroType
    {
        [BsonId]
        public ObjectId HeroTypeId { get; set; }

        public string HeroTypeName { get; set; }

        public bool HeroTypeDeleted { get; set; }

        [BsonIgnore]
        public ValidationResult ValidationResult { get; set; }

        public HeroType()
        {
            HeroTypeId = ObjectId.GenerateNewId();
            HeroTypeDeleted = false;
            ValidationResult = new ValidationResult();
        }

        public bool IsValid()
        {
            ValidationResult.Add(new HeroTypeIsConsistentValidation().Validate(this));
            return ValidationResult.IsValid;
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
