using DomainValidation.Interfaces.Specification;

namespace Storm.Domain.Specifications.Hero
{
    public class HeroHasValidTypeSpecification : ISpecification<Entities.Hero>
    {
        public bool IsSatisfiedBy(Entities.Hero entity)
        {
            return entity.HeroType != null;
        }
    }
}
