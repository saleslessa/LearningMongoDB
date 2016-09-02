using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Storm.Business.ViewModels
{
    public class PlayerViewModel
    {
        [BsonId]
        public ObjectId PlayerId { get; set; }

        public string PlayerName { get; set; }

        [DataType(DataType.Currency)]
        public double PlayerMoney { get; set; }

        public List<HeroViewModel> PlayerHeroes { get; set; }

        [BsonIgnore]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }

        public PlayerViewModel()
        {
            PlayerId = ObjectId.GenerateNewId();
            PlayerHeroes = new List<HeroViewModel>();
            ValidationResult = new DomainValidation.Validation.ValidationResult();
        }
        
    }
}
