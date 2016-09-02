using DomainValidation.Validation;
using MongoDB.Bson;
using Storm.Business.ViewModels;
using System.Collections.Generic;

namespace Storm.Business.Interfaces
{
    public interface IHeroTypeAppService
    {
        ValidationResult Add(HeroTypeViewModel model);

        HeroTypeViewModel GetById(ObjectId id);

        IEnumerable<HeroTypeViewModel> ListAll();

        HeroTypeViewModel GetByName(string name);

        ValidationResult Update(HeroTypeViewModel model);

        ValidationResult Update(HeroTypeViewModel model, Dictionary<string, string> keyValue);

        void Remove(ObjectId id);
    }
}
