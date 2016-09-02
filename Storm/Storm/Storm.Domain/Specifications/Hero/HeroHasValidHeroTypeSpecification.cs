using DomainValidation.Interfaces.Specification;
using Storm.Domain.Interfaces.Repository;

namespace Storm.Domain.Specifications.Hero
{
    public class HeroHasValidHeroTypeSpecification : ISpecification<Entities.Hero>
    {
        private readonly IHeroTypeRepository _heroTypeRepository;

        public HeroHasValidHeroTypeSpecification(IHeroTypeRepository heroTypeRepository)
        {
            _heroTypeRepository = heroTypeRepository;
        }

        public bool IsSatisfiedBy(Entities.Hero entity)
        {
            return _heroTypeRepository.GetByName(entity.HeroType.HeroTypeName) != null;
        }
    }
}
