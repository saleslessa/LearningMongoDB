using DomainValidation.Validation;
using MongoDB.Bson;
using Storm.Domain.Entities;
using System.Collections.Generic;

namespace Storm.Domain.Interfaces.Service
{
    public interface IPlayerService : IService
    {
        ValidationResult Add(Player model);

        Player GetById(ObjectId id);

        IEnumerable<Player> ListAll();

        Player GetByName(string name);

        ValidationResult Update(Player model);

        void Remove(ObjectId id);

    }
}
