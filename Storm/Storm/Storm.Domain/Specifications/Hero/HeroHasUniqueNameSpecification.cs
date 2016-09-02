using DomainValidation.Interfaces.Specification;
using Storm.Domain.Interfaces.Repository;

namespace Storm.Domain.Specifications.Hero
{
    public class HeroHasUniqueNameSpecification : ISpecification<Entities.Hero>
    {

        private readonly IHeroRepository _heroRepository;

        public HeroHasUniqueNameSpecification(IHeroRepository heroRepository)
        {
            _heroRepository = heroRepository;
        }

        public bool IsSatisfiedBy(Entities.Hero entity)
        {
            var obj = _heroRepository.GetByName(entity.HeroName);
            return obj == null || obj.HeroId == entity.HeroId;
        }
    }
}
