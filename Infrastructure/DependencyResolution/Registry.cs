using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Application.DependencyResolution
{
    public class Registry : StructureMap.Configuration.DSL.Registry
    {
        public Registry()
        {
            Scan(scan => {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}