using System.Web;
using GeoCMS.Application;
using OrangeCMS.Infrastructure;

namespace Application
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_BeginRequest()
        {
            ApplicationEvents.BeginRequest(Startup.StructureMapDependencyScope);
        } 

        protected void Application_EndRequest()
        {
            ApplicationEvents.EndRequest(Startup.StructureMapDependencyScope);
        }

        protected void Application_End()
        {
            ApplicationEvents.End(Startup.StructureMapDependencyScope);
        }
    }
}
