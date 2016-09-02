using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using Storm.Infra.CrossCutting.IoC;
using System.Web.Http;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Storm.Service.App_Start.SimpleInjectorInitializer), "Initialize")]
namespace Storm.Service.App_Start
{
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            
            InitializeContainer(container);
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            //container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

        private static void InitializeContainer(Container container)
        {
            Bootstrapper.RegisterServices(container);
        }
    }
}