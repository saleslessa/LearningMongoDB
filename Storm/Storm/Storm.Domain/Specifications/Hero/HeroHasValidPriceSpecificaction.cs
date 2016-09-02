using DomainValidation.Interfaces.Specification;

namespace Storm.Domain.Specifications.Hero
{
    class HeroHasValidPriceSpecificaction : ISpecification<Entities.Hero>
    {
        public bool IsSatisfiedBy(Entities.Hero entity)
        {
            return (entity.HeroFreeToPlay == true && entity.HeroPrice == 0) || (entity.HeroPrice <= 25 && entity.HeroPrice >= 5);
        }
    }
}
