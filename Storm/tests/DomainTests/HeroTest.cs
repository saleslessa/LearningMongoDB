using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storm.Domain.Entities;
using System.Linq;

namespace DomainTests
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

            //Act
            var result = hero.IsValid();
            var resultFree = heroFree.IsValid();


            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(resultFree);
        }

        //AAA - Arrange - Act - Assert
        [TestMethod]
        public void Hero_IsConsistent_False()
        {
            //Arrange
            var heroPriceBelow = new Hero()
            {
                HeroName = "hfghf ",
                HeroPrice = 4
            };

            var heroPriceAbove = new Hero()
            {
                HeroName = "hfghfgh",
                HeroPrice = 26
            };

            var hero = new Hero()
            {
                HeroName = "hfga////////434hfgh",
                HeroPrice = 26,
                HeroFreeToPlay = true
            };

            //Act
            var resultPriceBelow = heroPriceBelow.IsValid();
            var resultPriceAbove = heroPriceAbove.IsValid();

            hero.IsValid();
            var validationResult = hero.ValidationResult;

            //Assert
            Assert.IsFalse(resultPriceAbove);
            Assert.IsFalse(resultPriceBelow);

            Assert.IsTrue(validationResult.Erros.Any(a => a.Message == "Hero has invalid name. Please chose another."));
            Assert.IsTrue(validationResult.Erros.Any(a => a.Message == "Hero has invalid price. Please chose another."));
            Assert.IsTrue(validationResult.Erros.Any(a => a.Message == "Hero has invalid type. Please try again."));
        }
    }
}
