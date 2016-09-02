using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Storm.Business.ViewModels
{
    public class HeroTypeViewModel
    {
        [BsonId]
        public ObjectId HeroTypeId { get; set; }

        [DisplayName("Name")]
        public string HeroTypeName { get; set; }

        [ScaffoldColumn(false)]
        public bool HeroTypeDeleted { get; set; }

        [BsonIgnore]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }

        public HeroTypeViewModel()
        {
            HeroTypeId = ObjectId.GenerateNewId();
            HeroTypeDeleted = false;
            ValidationResult = new DomainValidation.Validation.ValidationResult();
        }
    }
}
