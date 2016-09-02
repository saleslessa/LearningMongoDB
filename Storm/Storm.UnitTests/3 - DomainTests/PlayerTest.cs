using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storm.Domain.Entities;
using System.Linq;

namespace Storm.UnitTests
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void PlayerIsConsistent_True()
        {
            var player = new Player()
            {
                PlayerName = "test"
            };

            Assert.IsTrue(player.IsValid());
        }

        [TestMethod]
        public void PlayerIsConsistent_False()
        {
            var player = new Player(){ };

            var result = player.IsValid();
            Assert.IsFalse(result);
            Assert.IsTrue(player.ValidationResult.Erros.Any(e => e.Message == "Player has invalid name. Please chose another."));
        }
    }
}
