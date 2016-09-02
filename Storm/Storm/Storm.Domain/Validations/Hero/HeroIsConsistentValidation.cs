using DomainValidation.Validation;
using Storm.Domain.Specifications.Hero;

namespace Storm.Domain.Validations.Hero
{
    public class HeroIsConsistentValidation : Validator<Entities.Hero>
    {
        public HeroIsConsistentValidation()
        {
            var validName = new HeroHasValidNameSpecification();
            var validPrice = new HeroHasValidPriceSpecificaction();
            var validType = new HeroHasValidTypeSpecification();

            base.Add("validName", new Rule<Entities.Hero>(validName, "Hero has invalid name. Please chose another."));
            base.Add("validPrice", new Rule<Entities.Hero>(validPrice, "Hero has invalid price. Please chose another."));
            base.Add("validType", new Rule<Entities.Hero>(validType, "Hero has invalid type. Please try again."));
        }
    }
}
