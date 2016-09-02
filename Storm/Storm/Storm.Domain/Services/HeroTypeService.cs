using Storm.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using DomainValidation.Validation;
using MongoDB.Bson;
using Storm.Domain.Entities;
using Storm.Domain.Interfaces.Repository;

namespace Storm.Domain.Services
{
    public class HeroTypeService : Service, IHeroTypeService
    {
        private readonly IHeroTypeRepository _heroTypeRepository;

        public HeroTypeService(IHeroTypeRepository heroTypeRepository)
        {
            _heroTypeRepository = heroTypeRepository;
        }

        public ValidationResult Add(HeroType model)
        {
            try
            {
                _heroTypeRepository.Add(model);
                model.ValidationResult.Message = "Hero Type added successfully";

                return model.ValidationResult;
            }
            catch (Exception ex)
            {
                var validation = new ValidationResult();
                validation.Add(new ValidationError(ex.Message));
                return validation;
            }
        }

        public HeroType GetById(ObjectId id)
        {
            return _heroTypeRepository.GetById(id);
        }

        public HeroType GetByName(string name)
        {
            return _heroTypeRepository.GetByName(name);
        }

        public IEnumerable<HeroType> ListAll()
        {
            return _heroTypeRepository.ListAll();
        }

        public void Remove(ObjectId id)
        {
            _heroTypeRepository.Remove(id);
        }

        public ValidationResult Update(HeroType model)
        {
            try
            {
                _heroTypeRepository.Update(model);
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
    }
}
