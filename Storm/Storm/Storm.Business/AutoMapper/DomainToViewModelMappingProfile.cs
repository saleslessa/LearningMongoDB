using AutoMapper;
using Storm.Business.ViewModels;
using Storm.Domain.Entities;

namespace Storm.Business.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Hero, HeroViewModel>();
            CreateMap<HeroType, HeroTypeViewModel>();
            CreateMap<Player, PlayerViewModel>();

        }
    }
}
