using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Storm.Domain.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Storm.Business.ViewModels
{
    public class HeroViewModel
    {
        [Key]
        public ObjectId HeroId { get; set; }

        [DisplayName("Hero Name")]
        public string HeroName { get; set; }

        [DisplayName("Type")]
        public virtual HeroTypeViewModel HeroType { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Creation Date")]
        public DateTime HeroCreationDate { get; set; }

        [DisplayName("Active")]
        public bool HeroActive { get; set; }

        [DisplayName("Deleted")]
        public bool HeroDeleted { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Price")]
        public double HeroPrice { get; set; }

        [DisplayName("Free?")]
        public bool HeroFreeToPlay { get; set; }

        [BsonIgnore]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }

        public HeroViewModel()
        {
            //HeroId = ObjectId.GenerateNewId();
            HeroDeleted = false;
            HeroFreeToPlay = false;
            ValidationResult = new DomainValidation.Validation.ValidationResult();
        }
    }
}
