using System.Collections.Generic;
using DomainValidation.Validation;
using Storm.Business.Interfaces;
using Storm.Business.ViewModels;
using Storm.Domain.Interfaces.Service;
using AutoMapper;
using Storm.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Storm.Business.AppService
{
    public class HeroAppService : IHeroAppService
    {
        private readonly IHeroService _heroService;

        public HeroAppService(IHeroService heroService)
        {
            _heroService = heroService;
        }

        public ValidationResult Add(HeroViewModel model)
        {
            var hero = Mapper.Map<HeroViewModel, Hero>(model);

            if (!hero.IsValid())
                return hero.ValidationResult;

            return _heroService.Add(hero);
        }

        public HeroViewModel GetById(ObjectId id)
        {
            return Mapper.Map<Hero, HeroViewModel>(_heroService.GetById(id));
        }

        public HeroViewModel GetByName(string name)
        {
            return Mapper.Map<Hero, HeroViewModel>(_heroService.GetByName(name));
        }

        public IEnumerable<HeroViewModel> ListAll()
        {
            return Mapper.Map<IEnumerable<Hero>, IEnumerable<HeroViewModel>>(_heroService.ListAll());
        }

        public void Remove(ObjectId id)
        {
            _heroService.Remove(id);
        }

        public ValidationResult Update(HeroViewModel model)
        {
            var hero = Mapper.Map<HeroViewModel, Hero>(model);

            if (!hero.IsValid())
                return hero.ValidationResult;

            return _heroService.Update(hero);
        }

        public ValidationResult Update(HeroViewModel model, Dictionary<string, string> keyValue)
        {
            model.ValidationResult = _heroService.ValidateModel(model, keyValue);
            if (!model.ValidationResult.IsValid)
                return model.ValidationResult;

            var hero = SetValues(Mapper.Map<HeroViewModel, Hero>(model), keyValue);

            if (hero.IsValid())
                return _heroService.Update(hero);

            return hero.ValidationResult;
        }

        private Hero SetValues(Hero model, Dictionary<string, string> keyValue)
        {
            var modelSerialized = model.ToBsonDocument();
           
            foreach (var item in keyValue)
                modelSerialized.SetElement(new BsonElement(item.Key, item.Value));

            try
            {
                return BsonSerializer.Deserialize<Hero>(modelSerialized);
            }
            catch
            {
                var validation = new ValidationResult();
                validation.Add(new ValidationError("Invalid values for update"));

                return new Hero()
                {
                    ValidationResult = validation
                };
            }
        }
    }
}
