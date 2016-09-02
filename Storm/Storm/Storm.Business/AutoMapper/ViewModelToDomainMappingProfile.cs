using AutoMapper;
using Storm.Business.ViewModels;
using Storm.Domain.Entities;

namespace Storm.Business.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<HeroViewModel, Hero>();
            CreateMap<HeroTypeViewModel, HeroType>();
            CreateMap<PlayerViewModel, Player>();
        }
    }
}
