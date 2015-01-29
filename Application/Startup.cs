using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using BundleTransformer.Core.Resolvers;
using CodeKinden.OrangeCMS.Application.DependencyResolution;
using CodeKinden.OrangeCMS.Application.Providers;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Services.Commands;
using CodeKinden.OrangeCMS.Domain.Services.Queries;
using GeoCMS.Infrastructure.Registries;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using OrangeCMS.Infrastructure;
using Owin;
using StructureMap;

namespace CodeKinden.OrangeCMS.Application
{
    public class Startup
    {
        public static MvcDependencyResolver StructureMapDependencyScope { get; set; }

        public static IContainer CreateContainer(Action<ConfigurationExpression> configure = null)
        {
            var container = new Container(c =>
            {
                c.AddRegistry<PersistenceRegistry>();
                c.AddRegistry<ApplicationRegistry>();
                c.For<IMappingEngine>().Use(() => Mapper.Engine);
                if (configure != null) configure.Invoke(c);
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

        public void Configuration(IAppBuilder app)
        {
            ConfigureApi(app);
            
            var isApiOnly = bool.Parse(ConfigurationManager.AppSettings["API.only"] ?? "false");

            if (isApiOnly) return;

            ConfigureMvc();
        }

        private void ConfigureApi(IAppBuilder app)
        {
            var container = CreateContainer();

            var config = new HttpConfiguration();

            SetupDependencyResolution(config, container);

            SetupOAuth(app, container);

            RegisterApiRoutes(config);

            app.UseWebApi(config);

            app.UseCors(CorsOptions.AllowAll);

            config.SuppressDefaultHostAuthentication();

            config.Filters.Add(new HostAuthenticationFilter("Bearer"));

            CreateSystemAdministrator(ConfigurationProvider.System.Username, ConfigurationProvider.System.Password, ConfigurationProvider.System.Email, container);
        }

        private static void ConfigureMvc()
        {
            RegisterMvcRoutes(RouteTable.Routes);
            BundleResolver.Current = new CustomBundleResolver();
            AssetProvider.BundleJs(BundleTable.Bundles);
            AssetProvider.BundleCss(BundleTable.Bundles);
        }

        private void CreateSystemAdministrator(string username, string password, string email, IContainer container)
        {
            var userQueries = container.GetInstance<IUserQueries>();
            var userCommands = container.GetInstance<IUserCommands>();
            if (userQueries.Exists(username)) return;
            userCommands.Save(username, password, email, Role.System).Wait();
        }

        private static void SetupOAuth(IAppBuilder app, IContainer container)
        {
            var OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/tokens"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = container.GetInstance<TokenProvider>()
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                Provider = new BearerAuthorizationProvider()
            });
        }

        private static void RegisterMvcRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
        }
        
        private static void RegisterApiRoutes(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.MapHttpAttributeRoutes();
        }

        private static void SetupDependencyResolution(HttpConfiguration configuration, IContainer container)
        {
            DependencyResolver.SetResolver(new MvcDependencyResolver(container));
            configuration.DependencyResolver = new WebApiDependencyResolver(container, false);
        }
    }
}