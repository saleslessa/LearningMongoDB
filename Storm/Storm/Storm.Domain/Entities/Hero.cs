using DomainValidation.Validation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Storm.Domain.Validations.Hero;
using System;


namespace Storm.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class Hero
    {
        [BsonId]
        public ObjectId HeroId { get; set; }

        public string HeroName { get; set; }

        public HeroType HeroType { get; set; }

        public DateTime HeroCreationDate { get; set; }

        public bool HeroActive { get; set; }

        public bool HeroDeleted { get; set; }

        public double HeroPrice { get; set; }

        public bool HeroFreeToPlay { get; set; }

        [BsonIgnore]
        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            ValidationResult.Add(new HeroIsConsistentValidation().Validate(this));
            return ValidationResult.IsValid;
        }

        public Hero()
        {
            HeroId = ObjectId.GenerateNewId();
            HeroDeleted = false;
            HeroFreeToPlay = false;
            ValidationResult = new ValidationResult();
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
