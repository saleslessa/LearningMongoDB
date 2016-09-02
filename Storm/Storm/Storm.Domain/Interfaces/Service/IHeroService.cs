using DomainValidation.Validation;
using MongoDB.Bson;
using Storm.Domain.Entities;
using System.Collections.Generic;

namespace Storm.Domain.Interfaces.Service
{
    public interface IHeroService : IService
    {
        ValidationResult Add(Hero model);

        Hero GetById(ObjectId id);

        IEnumerable<Hero> ListAll();

        Hero GetByName(string name);

        ValidationResult Update(Hero model);

        void Remove(ObjectId id);
    }
}
