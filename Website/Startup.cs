using System;
using System.Web.Routing;
using Application.DependencyResolution;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using OrangeCMS.Application;
using OrangeCMS.Application.Providers;
using Owin;
using System.Web.Http;
using StructureMap;

[assembly: OwinStartup(typeof(GeoCMS.Application.Startup))]
namespace GeoCMS.Application
{
    public class Startup
    {
        public static StructureMapDependencyScope StructureMapDependencyScope { get; set; }

        public void Configuration(IAppBuilder app)
        {
            IContainer container = new Container(c => c.AddRegistry<Registry>());
            StructureMapDependencyScope = new StructureMapDependencyScope(container);

            var config = new HttpConfiguration();

            app.UseWebApi(config);
            app.UseCors(CorsOptions.AllowAll);
            
            var OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/tokens"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = StructureMapDependencyScope.GetInstance<TokenProvider>()
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
 
            Bootstrapper.RegisterApiRoutes(config);
            Bootstrapper.SetupDependencyResolution(config, container, StructureMapDependencyScope);
            Bootstrapper.RegisterRoutes(RouteTable.Routes);

            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
        }
    }
}