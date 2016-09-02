using DomainValidation.Interfaces.Specification;

namespace Storm.Domain.Specifications.Player
{
    public class PlayerHasValidNameSpecification : ISpecification<Entities.Player>
    {
        public bool IsSatisfiedBy(Entities.Player entity)
        {
            return entity.PlayerName != null && entity.PlayerName.Trim().Length > 0;
        }
    }
}
