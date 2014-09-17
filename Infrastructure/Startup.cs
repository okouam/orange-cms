using System;
using System.Web.Mvc;
using System.Web.Routing;
using Application.DependencyResolution;
using AutoMapper;
using GeoCMS.Infrastructure.Registries;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using OrangeCMS.Application.Providers;
using Owin;
using System.Web.Http;
using StructureMap;

namespace Codeifier.OrangeCMS.Application
{
    public class Startup
    {
        public static StructureMapDependencyScope StructureMapDependencyScope { get; set; }

        public void Configuration(IAppBuilder app)
        {
            var container = CreateContainer();

            StructureMapDependencyScope = new StructureMapDependencyScope(container);

            var config = new HttpConfiguration();

            var OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/tokens"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = StructureMapDependencyScope.GetInstance<TokenProvider>()
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
                                             {
                                                     Provider = new BearerAuthorizationProvider()
                                             });
            
            RegisterApiRoutes(config);

            app.UseWebApi(config);
            app.UseCors(CorsOptions.AllowAll);

            SetupDependencyResolution(config, container, StructureMapDependencyScope);
            RegisterRoutes(RouteTable.Routes);

            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter("Bearer"));
        }
        
        public static IContainer CreateContainer(Action<ConfigurationExpression> configure = null)
        {
            var container = new Container(c =>
            {
                c.AddRegistry<PersistenceRegistry>();
                c.AddRegistry<ApplicationRegistry>();
                c.For<IMappingEngine>().Use(() => Mapper.Engine);
                if (configure != null) configure(c);
            });

            Mapper.Initialize(c =>
            {
                c.ConstructServicesUsing(container.GetInstance);
                foreach (var profile in container.GetAllInstances<Profile>())
                {
                    c.AddProfile(profile);
                }
            });

            return container;
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
        }
        
        private static void RegisterApiRoutes(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.MapHttpAttributeRoutes();
        }

        private static void SetupDependencyResolution(HttpConfiguration configuration, IContainer container, IServiceProvider scope)
        {
            DependencyResolver.SetResolver(scope);
            configuration.DependencyResolver = new StructureMapWebApiDependencyResolver(container);
        }
    }
}