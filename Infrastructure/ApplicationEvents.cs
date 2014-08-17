using Application.DependencyResolution;
using StructureMap.Web.Pipeline;

namespace OrangeCMS.Infrastructure
{
    public class ApplicationEvents
    {
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
    }
}
