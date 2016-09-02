using SimpleInjector;
using Storm.Business.AppService;
using Storm.Business.Interfaces;
using Storm.Domain.Interfaces.Repository;
using Storm.Domain.Interfaces.Service;
using Storm.Domain.Services;
using Storm.Infra.Data.Context;
using Storm.Infra.Data.Repository;

namespace Storm.Infra.CrossCutting.IoC
{
    public class Bootstrapper
    {
        public static void RegisterServices(Container container)
        {
            //Business
            container.Register<IHeroAppService, HeroAppService>();
            container.Register<IPlayerAppService, PlayerAppService>();
            container.Register<IHeroTypeAppService, HeroTypeAppService>();

            //Domain
            container.Register<IHeroService, HeroService>();
            container.Register<IHeroTypeService, HeroTypeService>();
            container.Register<IPlayerService, PlayerService>();

            //Repository
            container.Register<IHeroRepository, HeroRepository>();
            container.Register<IHeroTypeRepository, HeroTypeRepository>();
            container.Register<IPlayerRepository, PlayerRepository>();


            //context
            container.Register<StormContext>();

        }
    }
}
