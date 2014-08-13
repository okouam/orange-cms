using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Application.DependencyResolution;
using GeoCMS.Application;
using OrangeCMS.Application;
using StructureMap;

namespace Application
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_BeginRequest()
        {
            Bootstrapper.BeginRequest(Startup.StructureMapDependencyScope);
        } 

        protected void Application_EndRequest()
        {
            Bootstrapper.EndRequest(Startup.StructureMapDependencyScope);
        }

        protected void Application_End()
        {
            Bootstrapper.End(Startup.StructureMapDependencyScope);
        }
    }
}
