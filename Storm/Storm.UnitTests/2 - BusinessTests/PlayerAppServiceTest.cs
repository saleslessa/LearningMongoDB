using AutoMapper;
using DomainValidation.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Storm.Business.AppService;
using Storm.Business.AutoMapper;
using Storm.Business.ViewModels;
using Storm.Domain.Entities;
using Storm.Domain.Interfaces.Repository;
using Storm.Domain.Interfaces.Service;
using Storm.Domain.Services;
using System.Collections.Generic;
using System.Linq;

namespace Storm.UnitTests
{
    [TestClass]
    public class PlayerAppServiceTest
    {
        public PlayerAppServiceTest()
        {
            AutoMapperConfig.RegisterMappings();
        }

        [TestMethod]
        public void PlayerAppService_Add_True()
        {
            //Arrange
            var playerWithHero = new PlayerViewModel()
            {
                PlayerName = "test"
            };
            playerWithHero.PlayerHeroes.Add(new HeroViewModel()
            {
                HeroName = "hero",
                HeroPrice = 10,
                HeroType = new HeroTypeViewModel() { HeroTypeName = "test" }
            });

            var playerWithoutHero = new PlayerViewModel()
            {
                PlayerName = "test"
            };

            //Act
            var repo = MockRepository.GenerateStub<IHeroRepository>();
            repo.Stub(s => s.GetByName(playerWithHero.PlayerHeroes[0].HeroName)).Return(Mapper.Map<HeroViewModel, Hero>(playerWithHero.PlayerHeroes[0]));

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();

            var heroService = new HeroService(repo, repoType);
            var playerService = MockRepository.GenerateStub<IPlayerService>();
            var AppService = new PlayerAppService(playerService, heroService);

            playerWithHero.ValidationResult.Add(AppService.Add(playerWithHero));
            playerWithoutHero.ValidationResult.Add(AppService.Add(playerWithoutHero));

            Assert.IsTrue(playerWithHero.ValidationResult.IsValid);
            Assert.IsTrue(playerWithoutHero.ValidationResult.IsValid);
        }

        [TestMethod]
        public void PlayerAppService_Add_False()
        {
            //Arrange
            var playerWithHero = new PlayerViewModel()
            {
                PlayerName = "test"
            };
            playerWithHero.PlayerHeroes.Add(new HeroViewModel()
            {
                HeroName = "hero",
                HeroPrice = 10,
                HeroType = new HeroTypeViewModel() { HeroTypeName = "test" }
            });

            var playerInconsistent = new PlayerViewModel()
            {
                PlayerName = null
            };

            //Act
            var repo = MockRepository.GenerateStub<IHeroRepository>();
            repo.Stub(s => s.GetByName(playerWithHero.PlayerHeroes[0].HeroName)).Return(null);

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();

            var heroService = new HeroService(repo, repoType);
            var playerService = MockRepository.GenerateStub<IPlayerService>();
            var AppService = new PlayerAppService(playerService, heroService);

            playerWithHero.ValidationResult.Add(AppService.Add(playerWithHero));
            playerInconsistent.ValidationResult.Add(AppService.Add(playerInconsistent));

            Assert.IsFalse(playerWithHero.ValidationResult.IsValid);
            Assert.IsTrue(playerWithHero.ValidationResult.Erros.Any(e => e.Message == "Hero " + playerWithHero.PlayerHeroes[0].HeroName + " is inexistent or invalid"));

            Assert.IsFalse(playerInconsistent.ValidationResult.IsValid);
            Assert.IsTrue(playerInconsistent.ValidationResult.Erros.Any(e => e.Message == "Player has invalid name. Please chose another."));
        }

        [TestMethod]
        public void PlayerAppService_Update_True()
        {
            //Arrange
            var playerWithHero = new PlayerViewModel()
            {
                PlayerName = "test"
            };
            playerWithHero.PlayerHeroes.Add(new HeroViewModel()
            {
                HeroName = "hero",
                HeroPrice = 10,
                HeroType = new HeroTypeViewModel() { HeroTypeName = "test" }
            });

            var playerWithoutHero = new PlayerViewModel()
            {
                PlayerName = "test"
            };

            //Act
            var repo = MockRepository.GenerateStub<IHeroRepository>();
            repo.Stub(s => s.GetByName(playerWithHero.PlayerHeroes[0].HeroName)).Return(Mapper.Map<HeroViewModel, Hero>(playerWithHero.PlayerHeroes[0]));

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();

            var heroService = new HeroService(repo, repoType);
            var playerService = MockRepository.GenerateStub<IPlayerService>();
            var AppService = new PlayerAppService(playerService, heroService);

            playerWithHero.ValidationResult.Add(AppService.Update(playerWithHero));
            playerWithoutHero.ValidationResult.Add(AppService.Update(playerWithoutHero));

            Assert.IsTrue(playerWithHero.ValidationResult.IsValid);
            Assert.IsTrue(playerWithoutHero.ValidationResult.IsValid);
        }

        [TestMethod]
        public void PlayerAppService_Update_False()
        {
            //Arrange
            var playerWithHero = new PlayerViewModel()
            {
                PlayerName = "test"
            };
            playerWithHero.PlayerHeroes.Add(new HeroViewModel()
            {
                HeroName = "hero",
                HeroPrice = 10,
                HeroType = new HeroTypeViewModel() { HeroTypeName = "test" }
            });

            var playerInconsistent = new PlayerViewModel()
            {
                PlayerName = null
            };

            //Act
            var repo = MockRepository.GenerateStub<IHeroRepository>();
            repo.Stub(s => s.GetByName(playerWithHero.PlayerHeroes[0].HeroName)).Return(null);

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();

            var heroService = new HeroService(repo, repoType);
            var playerService = MockRepository.GenerateStub<IPlayerService>();
            var AppService = new PlayerAppService(playerService, heroService);

            playerWithHero.ValidationResult.Add(AppService.Update(playerWithHero));
            playerInconsistent.ValidationResult.Add(AppService.Update(playerInconsistent));

            Assert.IsFalse(playerWithHero.ValidationResult.IsValid);
            Assert.IsTrue(playerWithHero.ValidationResult.Erros.Any(e => e.Message == "Hero " + playerWithHero.PlayerHeroes[0].HeroName + " is inexistent or invalid"));

            Assert.IsFalse(playerInconsistent.ValidationResult.IsValid);
            Assert.IsTrue(playerInconsistent.ValidationResult.Erros.Any(e => e.Message == "Player has invalid name. Please chose another."));
        }

        [TestMethod]
        public void PlayerAppService_BuyHero_True()
        {
            //Arrange
            var player = new PlayerViewModel()
            {
                PlayerName = "test",
                PlayerMoney = 300
            };

            var hero = new HeroViewModel()
            {
                HeroName = "test",
                HeroPrice = 24
            };

            //Act
            var repoPlayer = MockRepository.GenerateStub<IPlayerRepository>();
            repoPlayer.Stub(s => s.GetById(player.PlayerId)).Return(Mapper.Map<PlayerViewModel, Player>(player));

            var repoHero = MockRepository.GenerateStub<IHeroRepository>();
            repoHero.Stub(s => s.GetById(hero.HeroId)).Return(Mapper.Map<HeroViewModel, Hero>(hero));

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();

            var heroService = new HeroService(repoHero, repoType);
            var playerService = new PlayerService(repoPlayer, heroService);
            var AppService = new PlayerAppService(playerService, heroService);

            player.ValidationResult.Add(AppService.BuyHero(player.PlayerId, hero.HeroId));

            Assert.IsTrue(player.ValidationResult.IsValid);
        }

        [TestMethod]
        public void PlayerAppService_BuyHero_Inconsistent_False()
        {
            //Arrange
            var player = new PlayerViewModel()
            {
                PlayerName = "test",
                PlayerMoney = 300
            };

            var hero = new HeroViewModel()
            {
                HeroName = "test",
                HeroPrice = 24
            };

            //Act
            var repoPlayer = MockRepository.GenerateStub<IPlayerRepository>();
            repoPlayer.Stub(s => s.GetById(player.PlayerId)).Return(null);

            var repoHero = MockRepository.GenerateStub<IHeroRepository>();
            repoHero.Stub(s => s.GetById(hero.HeroId)).Return(null);

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();

            var heroService = new HeroService(repoHero, repoType);
            var playerService = new PlayerService(repoPlayer, heroService);
            var AppService = new PlayerAppService(playerService, heroService);

            player.ValidationResult.Add(AppService.BuyHero(player.PlayerId, hero.HeroId));

            Assert.IsFalse(player.ValidationResult.IsValid);
            Assert.IsTrue(player.ValidationResult.Erros.Any(e => e.Message == "Invalid Player"));
            Assert.IsTrue(player.ValidationResult.Erros.Any(e => e.Message == "Invalid Hero"));
        }

        [TestMethod]
        public void PlayerAppService_BuyHero_WithoutMoney_False()
        {
            //Arrange
            var player = new PlayerViewModel()
            {
                PlayerName = "test",
                PlayerMoney = 23
            };

            var hero = new HeroViewModel()
            {
                HeroName = "test",
                HeroPrice = 24
            };

            //Act
            var repoPlayer = MockRepository.GenerateStub<IPlayerRepository>();
            repoPlayer.Stub(s => s.GetById(player.PlayerId)).Return(Mapper.Map<PlayerViewModel, Player>(player));

            var repoHero = MockRepository.GenerateStub<IHeroRepository>();
            repoHero.Stub(s => s.GetById(hero.HeroId)).Return(Mapper.Map<HeroViewModel, Hero>(hero));

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();

            var heroService = new HeroService(repoHero, repoType);
            var playerService = new PlayerService(repoPlayer, heroService);
            var AppService = new PlayerAppService(playerService, heroService);

            player.ValidationResult.Add(AppService.BuyHero(player.PlayerId, hero.HeroId));

            Assert.IsFalse(player.ValidationResult.IsValid);
            Assert.IsTrue(player.ValidationResult.Erros.Any(e => e.Message == "You don't have money to buy this hero. Please chose another"));
        }

        [TestMethod]
        public void PlayerAppService_BuyHero_SameHero_False()
        {
            //Arrange
            var player = new PlayerViewModel()
            {
                PlayerName = "test",
                PlayerMoney = 23
            };

            var hero = new HeroViewModel()
            {
                HeroName = "test",
                HeroPrice = 24
            };
            player.PlayerHeroes.Add(hero);

            //Act
            var repoPlayer = MockRepository.GenerateStub<IPlayerRepository>();
            repoPlayer.Stub(s => s.GetById(player.PlayerId)).Return(Mapper.Map<PlayerViewModel, Player>(player));

            var repoHero = MockRepository.GenerateStub<IHeroRepository>();
            repoHero.Stub(s => s.GetById(hero.HeroId)).Return(Mapper.Map<HeroViewModel, Hero>(hero));

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();

            var heroService = new HeroService(repoHero, repoType);
            var playerService = new PlayerService(repoPlayer, heroService);
            var AppService = new PlayerAppService(playerService, heroService);

            player.ValidationResult.Add(AppService.BuyHero(player.PlayerId, hero.HeroId));

            Assert.IsFalse(player.ValidationResult.IsValid);
            Assert.IsTrue(player.ValidationResult.Erros.Any(e => e.Message == "You already have this hero. Please chose another"));
        }

        [TestMethod]
        public void PlayerAppService_UpdateKeyValue_True()
        {
            //Arrange
            var player = new PlayerViewModel()
            {
                PlayerName = "test",
                PlayerMoney = 200
            };
            player.PlayerHeroes.Add(new HeroViewModel()
            {
                HeroName = "hero",
                HeroPrice = 10,
                HeroType = new HeroTypeViewModel() { HeroTypeName = "test" }
            });
            var keyValue = new Dictionary<string, string>();
            keyValue.Add("PlayerName", "asd");
            keyValue.Add("PlayerMoney", "230");

            //Act
            var repo = MockRepository.GenerateStub<IPlayerRepository>();
            //repo.Stub(s => s.GetByName(player.PlayerHeroes[0].HeroName)).Return(Mapper.Map<HeroViewModel, Hero>(player.PlayerHeroes[0]));

            var heroService = MockRepository.GenerateStub<IHeroService>();
            var playerService = new PlayerService(repo, heroService);
            var AppService = new PlayerAppService(playerService, heroService);

            player.ValidationResult.Add(AppService.Update(player, keyValue));

            Assert.IsTrue(player.ValidationResult.IsValid);
        }

        [TestMethod]
        public void PlayerAppService_UpdateKeyValue_False()
        {
            //Arrange
            var validation = new ValidationResult();
            var player = new PlayerViewModel()
            {
                PlayerName = "test",
                PlayerMoney = 200
            };
            var keyValueInvalidKey = new Dictionary<string, string>();
            keyValueInvalidKey.Add("PlayerMony", "230");

            var keyValueInvalidValue = new Dictionary<string, string>();
            keyValueInvalidValue.Add("PlayerName", "");

            var keyValueInconsistentValue = new Dictionary<string, string>();
            keyValueInconsistentValue.Add("PlayerMoney", "a");

            //Act
            var repo = MockRepository.GenerateStub<IPlayerRepository>();
            //repo.Stub(s => s.GetByName(player.PlayerHeroes[0].HeroName)).Return(Mapper.Map<HeroViewModel, Hero>(player.PlayerHeroes[0]));

            var heroService = MockRepository.GenerateStub<IHeroService>();
            var playerService = new PlayerService(repo, heroService);
            var AppService = new PlayerAppService(playerService, heroService);

            validation.Add(AppService.Update(player, keyValueInvalidKey));
            validation.Add(AppService.Update(player, keyValueInvalidValue));
            validation.Add(AppService.Update(player, keyValueInconsistentValue));


            //Assert
            Assert.IsFalse(validation.IsValid);
            Assert.IsTrue(validation.Erros.Any(e => e.Message == "Attribute PlayerMony does not exist"));
            Assert.IsTrue(validation.Erros.Any(e => e.Message == "Player has invalid name. Please chose another."));
            Assert.IsTrue(validation.Erros.Any(e => e.Message == "Invalid values for update"));
        }
    }
}
