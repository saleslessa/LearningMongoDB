using DomainValidation.Validation;
using MongoDB.Bson;
using Storm.Domain.Entities;
using System.Collections.Generic;

namespace Storm.Domain.Interfaces.Service
{
    public interface IHeroTypeService : IService
    {
        ValidationResult Add(HeroType model);

        HeroType GetById(ObjectId id);

        IEnumerable<HeroType> ListAll();

        HeroType GetByName(string name);

        ValidationResult Update(HeroType model);

        void Remove(ObjectId id);
    }
}
