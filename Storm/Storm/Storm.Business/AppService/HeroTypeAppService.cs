using Storm.Business.Interfaces;
using System;
using System.Collections.Generic;
using DomainValidation.Validation;
using MongoDB.Bson;
using Storm.Business.ViewModels;
using Storm.Domain.Interfaces.Service;
using Storm.Domain.Entities;
using AutoMapper;
using MongoDB.Bson.Serialization;

namespace Storm.Business.AppService
{
    public class HeroTypeAppService : IHeroTypeAppService
    {
        private readonly IHeroTypeService _heroTypeService;

        public HeroTypeAppService(IHeroTypeService heroTypeService)
        {
            _heroTypeService = heroTypeService;
        }

        public ValidationResult Add(HeroTypeViewModel model)
        {
            var heroType = Mapper.Map<HeroTypeViewModel, HeroType>(model);

            if (heroType.IsValid())
                return _heroTypeService.Add(heroType);

            return heroType.ValidationResult;
        }

        public HeroTypeViewModel GetById(ObjectId id)
        {
            return Mapper.Map<HeroType, HeroTypeViewModel>(_heroTypeService.GetById(id));
        }

        public HeroTypeViewModel GetByName(string name)
        {
            return Mapper.Map<HeroType, HeroTypeViewModel>(_heroTypeService.GetByName(name));
        }

        public IEnumerable<HeroTypeViewModel> ListAll()
        {
            return Mapper.Map<IEnumerable<HeroType>, IEnumerable<HeroTypeViewModel>>(_heroTypeService.ListAll());
        }

        public void Remove(ObjectId id)
        {
            _heroTypeService.Remove(id);
        }

        public ValidationResult Update(HeroTypeViewModel model)
        {
            var heroType = Mapper.Map<HeroTypeViewModel, HeroType>(model);

            if (!heroType.IsValid())
                return heroType.ValidationResult;

            return _heroTypeService.Update(heroType);
        }

        public ValidationResult Update(HeroTypeViewModel model, Dictionary<string, string> keyValue)
        {
            model.ValidationResult = _heroTypeService.ValidateModel(model, keyValue);
            if (!model.ValidationResult.IsValid)
                return model.ValidationResult;

            var heroType = SetValues(Mapper.Map<HeroTypeViewModel, HeroType>(model), keyValue);

            if (heroType.IsValid())
                return _heroTypeService.Update(heroType);

            return heroType.ValidationResult;
        }

        private HeroType SetValues(HeroType model, Dictionary<string, string> keyValue)
        {
            var modelSerialized = model.ToBsonDocument();

            foreach (var item in keyValue)
                modelSerialized.SetElement(new BsonElement(item.Key, item.Value));

            try
            {
                return BsonSerializer.Deserialize<HeroType>(modelSerialized);
            }
            catch
            {
                var validation = new ValidationResult();
                validation.Add(new ValidationError("Invalid values for update"));

                return new HeroType()
                {
                    ValidationResult = validation
                };
            }
        }
    }
}
