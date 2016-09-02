using DomainValidation.Validation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Storm.Domain.Validations.Player;
using System.Collections.Generic;

namespace Storm.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class Player
    {
        [BsonId]
        public ObjectId PlayerId { get; set; }

        public string PlayerName { get; set; }

        public double PlayerMoney { get; set; }

        public List<Hero> PlayerHeroes { get; set; }

        [BsonIgnore]
        public ValidationResult ValidationResult { get; set; }

        public Player()
        {
            PlayerId = ObjectId.GenerateNewId();
            PlayerHeroes = new List<Hero>();
            ValidationResult = new ValidationResult();
        }

        public bool IsValid()
        {
            ValidationResult.Add(new PlayerIsConsistentValidation().Validate(this));
            return ValidationResult.IsValid;
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
