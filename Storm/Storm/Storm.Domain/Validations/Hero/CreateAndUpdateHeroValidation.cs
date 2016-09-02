using DomainValidation.Validation;
using Storm.Domain.Interfaces.Repository;
using Storm.Domain.Specifications.Hero;

namespace Storm.Domain.Validations.Hero
{
    public class CreateAndUpdateHeroValidation : Validator<Entities.Hero>
    {
        public CreateAndUpdateHeroValidation(IHeroRepository heroRepository, IHeroTypeRepository heroTypeRepository)
        {
            var uniqueName = new HeroHasUniqueNameSpecification(heroRepository);
            var validHeroType = new HeroHasValidHeroTypeSpecification(heroTypeRepository);

            base.Add("uniqueName", new Rule<Entities.Hero>(uniqueName, "This name was already chosen. Please chose another."));
            base.Add("validHeroType", new Rule<Entities.Hero>(validHeroType, "Invalid Hero Type. Please chose another."));
        }
    }
}
