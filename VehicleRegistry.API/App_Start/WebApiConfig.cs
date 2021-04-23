using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Web.Http;
using VehicleRegistry.Domain.Interfaces;
using VehicleRegistry.Domain.Classes;
using SimpleInjector.Integration.WebApi;
using VehicleRegistry.Repository.Interfaces;
using VehicleRegistry.Repository.Classes;

namespace VehicleRegistry.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static void RegisterSimpleInjector(HttpConfiguration config)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.Register<IVehicleService, VehicleService>(Lifestyle.Scoped);
            container.Register<IVehicleRepository, AzureRepository>(Lifestyle.Scoped);
            container.Register<IAccountRepository, AzureRepository>(Lifestyle.Scoped);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
