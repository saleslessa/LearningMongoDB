using DomainValidation.Validation;
using Storm.Domain.Specifications.HeroType;

namespace Storm.Domain.Validations.HeroType
{
    public class HeroTypeIsConsistentValidation : Validator<Entities.HeroType>
    {
        public HeroTypeIsConsistentValidation()
        {
            var validName = new HeroTypeHasValidNameSpecification();

            base.Add("validName", new Rule<Entities.HeroType>(validName, "Hero Type has invalid name. Please chose another."));
        }
    }
}
