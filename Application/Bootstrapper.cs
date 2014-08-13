using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Application.DependencyResolution;
using Newtonsoft.Json.Serialization;
using StructureMap;
using StructureMap.Web.Pipeline;

namespace OrangeCMS.Application
{
    public class Bootstrapper
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
        }

        public static void RegisterApiRoutes(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.MapHttpAttributeRoutes();

        }

        public static void BeginRequest(StructureMapDependencyScope scope)
        {
            scope.CreateNestedContainer();
        }

        public static void EndRequest(StructureMapDependencyScope scope)
        {
            HttpContextLifecycle.DisposeAndClearAll();
            scope.DisposeNestedContainer();
        }

        public static void End(StructureMapDependencyScope scope)
        {
            scope.Dispose();    
        }

        public static void SetupDependencyResolution(HttpConfiguration configuration, IContainer container, StructureMapDependencyScope scope)
        {
            DependencyResolver.SetResolver(scope);
            configuration.DependencyResolver = new StructureMapWebApiDependencyResolver(container);
        }
    }
}
