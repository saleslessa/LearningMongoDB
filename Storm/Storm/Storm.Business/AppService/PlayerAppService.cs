using Storm.Business.Interfaces;
using System.Collections.Generic;
using DomainValidation.Validation;
using MongoDB.Bson;
using Storm.Business.ViewModels;
using AutoMapper;
using Storm.Domain.Entities;
using Storm.Domain.Interfaces.Service;
using MongoDB.Bson.Serialization;
using System.Linq;

namespace Storm.Business.AppService
{
    public class PlayerAppService : IPlayerAppService
    {
        private readonly IPlayerService _playerService;
        private readonly IHeroService _heroService;

        public PlayerAppService(IPlayerService playerService, IHeroService heroService)
        {
            _playerService = playerService;
            _heroService = heroService;
        }

        public ValidationResult Add(PlayerViewModel model)
        {
            var player = Mapper.Map<PlayerViewModel, Player>(model);

            if (!player.IsValid())
                return player.ValidationResult;

            ValidateHeroes(ref player);

            if (!player.ValidationResult.IsValid)
                return player.ValidationResult;

            player.ValidationResult.Add(_playerService.Add(player));

            return player.ValidationResult;
        }

        private void ValidateHeroes(ref Player player)
        {
            for (int i = 0; i < player.PlayerHeroes.Count; i++)
            {
                var hero = _heroService.GetByName(player.PlayerHeroes[i].HeroName);

                if (!(hero == null || !hero.IsValid()))
                    player.PlayerHeroes[i] = hero;
                else
                    player.ValidationResult.Add(new ValidationError("Hero " + player.PlayerHeroes[i].HeroName + " is inexistent or invalid"));
            }
        }

        public PlayerViewModel GetById(ObjectId id)
        {
            return Mapper.Map<Player, PlayerViewModel>(_playerService.GetById(id));
        }

        public PlayerViewModel GetByName(string name)
        {
            return Mapper.Map<Player, PlayerViewModel>(_playerService.GetByName(name));
        }

        public IEnumerable<PlayerViewModel> ListAll()
        {
            return Mapper.Map<IEnumerable<Player>, IEnumerable<PlayerViewModel>>(_playerService.ListAll());
        }

        public void Remove(ObjectId id)
        {
            _playerService.Remove(id);
        }

        public ValidationResult Update(PlayerViewModel model)
        {
            var player = Mapper.Map<PlayerViewModel, Player>(model);

            if (!player.IsValid())
                return player.ValidationResult;

            ValidateHeroes(ref player);

            if (!player.ValidationResult.IsValid)
                return player.ValidationResult;

            player.ValidationResult.Add(_playerService.Update(player));

            return player.ValidationResult;
        }

        public ValidationResult Update(PlayerViewModel model, Dictionary<string, string> keyValue)
        {
            model.ValidationResult = _playerService.ValidateModel(model, keyValue);
            if (!model.ValidationResult.IsValid)
                return model.ValidationResult;

            var player = SetValues(Mapper.Map<PlayerViewModel, Player>(model), keyValue);

            if (player.IsValid())
                return Update(Mapper.Map<Player, PlayerViewModel>(player));

            return player.ValidationResult;
        }

        private Player SetValues(Player model, Dictionary<string, string> keyValue)
        {
            var modelSerialized = model.ToBsonDocument();

            foreach (var item in keyValue)
                modelSerialized.SetElement(new BsonElement(item.Key, item.Value));

            try
            {
                return BsonSerializer.Deserialize<Player>(modelSerialized);
            }
            catch
            {
                var validation = new ValidationResult();
                validation.Add(new ValidationError("Invalid values for update"));

                return new Player()
                {
                    ValidationResult = validation
                };
            }
        }

        public IEnumerable<HeroViewModel> GetAllHeroes(ObjectId id)
        {
            var player = _playerService.GetById(id);
            return Mapper.Map<IEnumerable<Hero>, IEnumerable<HeroViewModel>>(player.PlayerHeroes);
        }

        public IEnumerable<HeroViewModel> GetActiveHeroes(ObjectId id)
        {
            var result = new List<Hero>();
            var player = _playerService.GetById(id);

            foreach (var item in player.PlayerHeroes)
            {
                if (item.HeroActive)
                    result.Add(item);
            }

            return Mapper.Map<IEnumerable<Hero>, IEnumerable<HeroViewModel>>(result);
        }

        public ValidationResult BuyHero(ObjectId idPlayer, ObjectId idHero)
        {
            var validationResult = new ValidationResult();
            var player = _playerService.GetById(idPlayer);
            var hero = _heroService.GetById(idHero);

            ValidateConsistency(player, hero, ref validationResult);
            if (!validationResult.IsValid)
                return validationResult;

            player.PlayerMoney -= hero.HeroFreeToPlay == false ? hero.HeroPrice : 0;
            player.PlayerHeroes.Add(hero);

            if (_playerService.Update(player).IsValid)
                validationResult.Message = "Hero bought successfully";

            return validationResult;
        }

        private void ValidateConsistency(Player player, Hero hero, ref ValidationResult validation)
        {
            ValidateIfEntitiesExists(player, hero, ref validation);
            if (validation.IsValid)
                ValidateBuyItem(player, hero, ref validation);
        }

        private void ValidateIfEntitiesExists(Player player, Hero hero, ref ValidationResult validation)
        {
            if (player == null)
                validation.Add(new ValidationError("Invalid Player"));

            if (hero == null)
                validation.Add(new ValidationError("Invalid Hero"));
        }

        private void ValidateBuyItem(Player player, Hero hero, ref ValidationResult validation)
        {
            if (player.PlayerHeroes.Where(t => t.HeroId == hero.HeroId).ToList().Count > 0)
            {
                validation.Add(new ValidationError("You already have this hero. Please chose another"));
            }
            else
            {
                if (player.PlayerMoney < hero.HeroPrice && hero.HeroFreeToPlay == false)
                    validation.Add(new ValidationError("You don't have money to buy this hero. Please chose another"));
            }
        }
    }
}
