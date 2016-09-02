using DomainValidation.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storm.Domain.Entities;
using System.Linq;

namespace Storm.UnitTests
{
    [TestClass]
    public class HeroTest
    {
        //AAA - Arrange - Act - Assert
        [TestMethod]
        public void Hero_IsConsistent_True()
        {
            //Arrange
            var hero = new Hero() {
                HeroName = "aA zFDfdZ ",
                HeroPrice = 10,
                HeroType = new HeroType()
            };

            var heroFree = new Hero()
            {
                HeroName = "aA zFDfdZ ",
                HeroPrice = 0,
                HeroFreeToPlay = true,
                HeroType = new HeroType()
            };

            //Act and Assert
            Assert.IsTrue(hero.IsValid());
            Assert.IsTrue(heroFree.IsValid());
        }

        //AAA - Arrange - Act - Assert
        [TestMethod]
        public void Hero_IsConsistent_False()
        {
            var validationResult = new ValidationResult();
            //Arrange
            var heroPriceBelow = new Hero()
            {
                HeroName = "hfghf ",
                HeroPrice = 5
            };

            var hero = new Hero()
            {
                HeroName = "hfga////////434hfgh",
                HeroPrice = 26,
                HeroFreeToPlay = true
            };

            //Act
            heroPriceBelow.IsValid();
            validationResult.Add(heroPriceBelow.ValidationResult);

            hero.IsValid();
            validationResult.Add(hero.ValidationResult);

            //Assert
            Assert.IsFalse(validationResult.IsValid);

            Assert.IsTrue(validationResult.Erros.Any(a => a.Message == "Hero has invalid name. Please chose another."));
            Assert.IsTrue(validationResult.Erros.Any(a => a.Message == "Hero has invalid price. Please chose another."));
            Assert.IsTrue(validationResult.Erros.Any(a => a.Message == "Hero has invalid type. Please try again."));
        }
    }
}
