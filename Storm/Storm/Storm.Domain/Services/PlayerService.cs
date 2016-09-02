using Storm.Domain.Interfaces.Service;
using System.Collections.Generic;
using DomainValidation.Validation;
using MongoDB.Bson;
using Storm.Domain.Entities;
using Storm.Domain.Interfaces.Repository;
using System.Linq;

namespace Storm.Domain.Services
{
    public class PlayerService : Service, IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IHeroService _heroService;

        public PlayerService(IPlayerRepository playerRepository, IHeroService heroService)
        {
            _playerRepository = playerRepository;
            _heroService = heroService;
        }

        public ValidationResult Add(Player model)
        {
            _playerRepository.Add(model);
            model.ValidationResult.Message = "Player added successfully";
            return model.ValidationResult;
        }

        public Player GetById(ObjectId id)
        {
            var obj = _playerRepository.GetById(id);
            for (int i = 0; i < obj.PlayerHeroes.Count; i++)
                obj.PlayerHeroes[i] = _heroService.GetById(obj.PlayerHeroes[i].HeroId);

            return obj;
        }

        public Player GetByName(string name)
        {
            var obj = _playerRepository.GetByName(name);
            for (int i = 0; i < obj.PlayerHeroes.Count; i++)
                obj.PlayerHeroes[i] = _heroService.GetById(obj.PlayerHeroes[i].HeroId);

            return obj;
        }

        public IEnumerable<Player> ListAll()
        {
            var list = _playerRepository.ListAll().ToList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = GetById(list[i].PlayerId);
            }

            return list;
        }

        public void Remove(ObjectId id)
        {
            _playerRepository.Remove(id);
        }

        public ValidationResult Update(Player model)
        {
            if (model.IsValid())
            {
                _playerRepository.Update(model);
                model.ValidationResult.Message = "Player updated successfully";
            }

            return model.ValidationResult;
        }
    }
}
