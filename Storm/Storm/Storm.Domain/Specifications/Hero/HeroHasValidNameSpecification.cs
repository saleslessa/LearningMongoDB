using DomainValidation.Interfaces.Specification;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Storm.Domain.Specifications.Hero
{
    public class HeroHasValidNameSpecification : ISpecification<Entities.Hero>
    {
        public bool IsSatisfiedBy(Entities.Hero entity)
        {
            Regex objAlphaPattern = new Regex("^[a-zA-Z ]*$");
            return entity.HeroName != null && (objAlphaPattern.IsMatch(entity.HeroName));

            //return entity.HeroName != null && entity.HeroName.Any(t => !Char.IsLetter(t));
        }
    }
}
