using DomainValidation.Validation;
using MongoDB.Bson;
using Storm.Business.ViewModels;
using System;
using System.Collections.Generic;

namespace Storm.Business.Interfaces
{
    public interface IHeroAppService
    {
        ValidationResult Add(HeroViewModel model);

        HeroViewModel GetById(ObjectId id);

        IEnumerable<HeroViewModel> ListAll();

        HeroViewModel GetByName(string name);

        ValidationResult Update(HeroViewModel model);

        ValidationResult Update(HeroViewModel model, Dictionary<string, string> keyValue);

        void Remove(ObjectId id);
    }
}
