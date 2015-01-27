using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace CodeKinden.OrangeCMS.Application.DependencyResolution 
{
    public class MvcDependencyResolver : IDependencyResolver {

        private readonly IContainer container;

        public MvcDependencyResolver(IContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null) throw new ArgumentNullException("serviceType");

            // As explained above, this method will be called by the ASP.NET MVC framework to
            // resolve all sort of different types, including some that we won't know how to resolve.
            // If we're asked to resolve a type that's unknown to us, we must return null and not throw.
            // So:

            // 2) If we're asked to resolve an abstract type or an interface and we have a concrete type configured
            // in our container for it, we're good and can return that. Otherwise, it means that
            // we don't know anything about the type we've been asked to resolve. In which case,
            // we must return null and ASP.NET MVC will take care of using a default implementation 
            // of that type.
            if (serviceType.IsAbstract || serviceType.IsInterface)
            {
                return container.TryGetInstance(serviceType);
            }

            // 3) If we've been asked to resolve a concrete type, we can ask StructureMap 
            // to resolve it even if it's a type that's unknown to us. This is because StructureMap
            // will resolve concrete types regardless of whether or not they've been explicitly
            // configured in the container. In that case, we want to throw if any error 
            // occurs. Any error there would indicate a dependency issue and we most definitely
            // want the application to crash if that's the case. 
            return container.GetInstance(serviceType);

            // Important note to the maintainer: remember that with StructureMap, GetInstance() and
            // TryGetInstance() have a different behavior when it comes to resolving concrete types
            // that haven't been explicitly configured in the container. GetInstance() will in that case
            // create and return an instance of the requested type anyway. TryGetInstance() on the other
            // side will return null.
            // So beware of changing the code above to use TryGetInstance() instead of GetInstance()
            // when we've been asked to resolve a concrete type as it may break things. For example, 
            // our Controller classes may not have been explicitly registered in our StructureMap 
            // container (since there's no need to really). In that case, trying to resolve a Controller with 
            // GetInstance() will work. Trying to resolve a Controller with TryGetInstance() will 
            // return null. Boom.
            // So make sure that you fully understand how both StructureMap and ASP.NET MVC work 
            // before going and changing the code above. This is no place for quick-and-dirty hacks.
            // There is very little code here but it's been very carefully written.
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            // As noted in the large NOTE above, this method must neither throw
            // nor return null if asked to resolve a Type that's unknown to us.
            // It must return an empty list instead. 
            // If the Type is known to use but an error occurs when resolving 
            // a dependency, then we must of course throw as we want the application
            // to crash in this case.
            return container.GetAllInstances(serviceType).Cast<object>();
        }
    }
}
