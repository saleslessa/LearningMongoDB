using DomainValidation.Interfaces.Specification;
using System;

namespace Storm.Domain.Specifications.HeroType
{
    public class HeroTypeHasValidNameSpecification : ISpecification<Entities.HeroType>
    {
        public bool IsSatisfiedBy(Entities.HeroType entity)
        {
            return entity.HeroTypeName != null && entity.HeroTypeName.Trim().Length > 0;
        }
    }
}
