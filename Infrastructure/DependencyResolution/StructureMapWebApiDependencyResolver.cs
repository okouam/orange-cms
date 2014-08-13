using System.Web.Http.Dependencies;
using StructureMap;

namespace Application.DependencyResolution
{
    public class StructureMapWebApiDependencyResolver : StructureMapWebApiDependencyScope, IDependencyResolver
    {
        public StructureMapWebApiDependencyResolver(IContainer container) : base(container) 
        {
        }
        
        public IDependencyScope BeginScope()
        {
            var child = Container.GetNestedContainer();
            return new StructureMapWebApiDependencyResolver(child);
        }
    }
}