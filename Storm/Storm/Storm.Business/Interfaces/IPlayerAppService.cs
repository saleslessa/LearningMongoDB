using DomainValidation.Validation;
using MongoDB.Bson;
using Storm.Business.ViewModels;
using System.Collections.Generic;

namespace Storm.Business.Interfaces
{
    public interface IPlayerAppService
    {
        ValidationResult Add(PlayerViewModel model);

        PlayerViewModel GetById(ObjectId id);

        IEnumerable<PlayerViewModel> ListAll();

        PlayerViewModel GetByName(string name);

        ValidationResult Update(PlayerViewModel model);

        ValidationResult Update(PlayerViewModel model, Dictionary<string, string> keyValue);

        void Remove(ObjectId id);

        IEnumerable<HeroViewModel> GetAllHeroes(ObjectId id);

        IEnumerable<HeroViewModel> GetActiveHeroes(ObjectId id);

        ValidationResult BuyHero(ObjectId idPlayer, ObjectId idHero);
    }
}
