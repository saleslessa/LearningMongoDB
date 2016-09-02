using Storm.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using Storm.Domain.Entities;
using Storm.Domain.Interfaces.Repository;
using Storm.Domain.Validations.Hero;
using DomainValidation.Validation;
using MongoDB.Bson;
using System.Linq;

namespace Storm.Domain.Services
{
    public class HeroService : Service, IHeroService
    {
        private readonly IHeroRepository _heroRepository;
        private readonly IHeroTypeRepository _heroTypeRepository;

        public HeroService(IHeroRepository heroRepository, IHeroTypeRepository heroTypeRepository)
        {
            _heroRepository = heroRepository;
            _heroTypeRepository = heroTypeRepository;
        }

        public ValidationResult Add(Hero model)
        {
            try
            {
                if (ValidateCreationAndUpdate(ref model))
                {
                    _heroRepository.Add(model);
                    model.ValidationResult.Message = "Hero added successfully";
                }

                return model.ValidationResult;
            }
            catch (Exception ex)
            {
                var validation = new ValidationResult();
                validation.Add(new ValidationError(ex.Message));
                return validation;
            }
        }

        public Hero GetById(ObjectId id)
        {
            var obj = _heroRepository.GetById(id);
            obj.HeroType = _heroTypeRepository.GetById(obj.HeroType.HeroTypeId);
            return obj;
        }

        public Hero GetByName(string name)
        {
            var obj = _heroRepository.GetByName(name);
            obj.HeroType = _heroTypeRepository.GetById(obj.HeroType.HeroTypeId);
            return obj;
        }

        public IEnumerable<Hero> ListAll()
        {
            var obj = _heroRepository.ListAll().ToList();
            for (int i = 0; i < obj.Count; i++)
            {
                obj[i].HeroType = _heroTypeRepository.GetById(obj[i].HeroType.HeroTypeId);
            }

            return obj;
        }

        public void Remove(ObjectId id)
        {
            _heroRepository.Remove(id);
        }

        public ValidationResult Update(Hero model)
        {
            try
            {
                if (!ValidateCreationAndUpdate(ref model))
                    return model.ValidationResult;

                _heroRepository.Update(model);
                model.ValidationResult.Message = "Hero updated successfully";
                return model.ValidationResult;
            }
            catch (Exception ex)
            {
                var validation = new ValidationResult();
                validation.Add(new ValidationError(ex.Message));
                return validation;
            }
        }

        private bool ValidateCreationAndUpdate(ref Hero model)
        {
            //Verifying business rules
            model.ValidationResult = new CreateAndUpdateHeroValidation(_heroRepository, _heroTypeRepository).Validate(model);
            if (!model.ValidationResult.IsValid)
                return false;

            return true;
        }


    }
}
