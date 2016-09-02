﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storm.Domain.Entities;
using Rhino.Mocks;
using Storm.Domain.Interfaces.Repository;
using Storm.Domain.Validations.Hero;
using DomainValidation.Validation;
using System.Linq;

namespace Storm.UnitTests
{
    [TestClass]
    public class HeroRepositoryTest
    {
        //AAA - Arrange - Act - Assert
        [TestMethod]
        public void HeroRepository_Add_True()
        {
            //Arrange
            var hero = new Hero()
            {
                HeroName = "test",
                HeroPrice = 15,
                HeroType = new HeroType() { HeroTypeName = "test" }
            };

            //Act
            var repo = MockRepository.GenerateStub<IHeroRepository>();
            repo.Stub(s => s.GetByName(hero.HeroName)).Return(null);

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();
            repoType.Stub(s => s.GetByName("test")).Return(new HeroType());

            var validationResult = new CreateAndUpdateHeroValidation(repo, repoType).Validate(hero);

            //Assert
            Assert.IsTrue(validationResult.IsValid);
        }

        //AAA - Arrange - Act - Assert
        [TestMethod]
        public void HeroRepository_AddAndUpdate_False()
        {
            //Arrange
            var hero = new Hero()
            {
                HeroName = "test",
                HeroPrice = 15,
                HeroType = new HeroType()
            };

            //Act
            var repo = MockRepository.GenerateStub<IHeroRepository>();
            repo.Stub(s => s.GetByName(hero.HeroName)).Return(new Hero());

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();

            var validationResult = new CreateAndUpdateHeroValidation(repo, repoType).Validate(hero);

            //Assert
            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationResult.Erros.Any(a => a.Message == "This name was already chosen. Please chose another."));
        }

        //AAA - Arrange - Act - Assert
        [TestMethod]
        public void HeroRepository_Update_True()
        {
            var validationResult = new ValidationResult();
            //Arrange
            var returnSameHero = new Hero()
            {
                HeroName = "test",
                HeroPrice = 15,
                HeroType = new HeroType() { HeroTypeName = "test" }
            };
            //Arrange
            var returnNull = new Hero()
            {
                HeroName = "test",
                HeroPrice = 15,
                HeroType = new HeroType() { HeroTypeName = "test" }
            };

            //Act
            var repoSameHero = MockRepository.GenerateStub<IHeroRepository>();
            repoSameHero.Stub(s => s.GetByName(returnSameHero.HeroName)).Return(returnSameHero);

            var repoNull = MockRepository.GenerateStub<IHeroRepository>();
            repoNull.Stub(s => s.GetByName(returnNull.HeroName)).Return(null);

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();
            repoType.Stub(s => s.GetByName("test")).Return(new HeroType());


            validationResult.Add(new CreateAndUpdateHeroValidation(repoSameHero, repoType).Validate(returnSameHero));
            validationResult.Add(new CreateAndUpdateHeroValidation(repoNull, repoType).Validate(returnNull));

            //Assert
            Assert.IsTrue(validationResult.IsValid);
        }
    }
}
