using DomainValidation.Validation;
using Storm.Domain.Specifications.Player;

namespace Storm.Domain.Validations.Player
{
    public class PlayerIsConsistentValidation : Validator<Entities.Player>
    {
        public PlayerIsConsistentValidation()
        {
            var validName = new PlayerHasValidNameSpecification();

            base.Add("validName", new Rule<Entities.Player>(validName, "Player has invalid name. Please chose another."));
        }
    }
}
