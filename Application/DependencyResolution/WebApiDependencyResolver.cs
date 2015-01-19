using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using StructureMap;

namespace Application.DependencyResolution
{
    public class WebApiDependencyResolver : IDependencyResolver
    {
        private bool disposed;
        private readonly IContainer container;
        private readonly bool isNestedResolver;

        /// <summary>
        /// Creates a new instance of a WebAPI IDependencyResolver that uses
        /// StructureMap to resolve the requested components.
        /// </summary>
        public WebApiDependencyResolver(IContainer container, bool isNestedResolver)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.container = container;
            this.isNestedResolver = isNestedResolver;
        }

        /*
         * NOTE:
         * 
         * Since I couldn't find an authoritative answer on whether the WebAPI framework
         * has the same behavior as the ASP.NET MVC framework when it comes to dependency
         * resolution, I'm going to assume that both frameworks have the same behavior.
         * 
         * So refer to the comments in the MvcDependencyResolver class for more information
         * on how the framework uses this dependency resolver. This will explain what we're
         * doing here and why we're doing it that way.
         * 
         * One obvious difference of course is that the WebAPI framework supports the concept 
         * of nested dependency resolution scopes via the BeginScope() and Dispose() methods.
         * StructureMap has built-in support for that so that's easy to add. 
         * 
         */

        public object GetService(Type serviceType)
        {
            if (disposed) throw new ObjectDisposedException("WebApiDependencyResolver");
            if (serviceType == null) throw new ArgumentNullException("serviceType");

            // This method will be called by the WebAPI framework to
            // resolve all sort of different types, including some that we won't know how to resolve.
            // See the comments in the MvcDependencyResolver class for more details on this.
            // 
            // If we're asked to resolve a type that's unknown to us, we must return null and not throw.
            // So:

            // 1) If we know for sure that the component we're asked to resolve is one of ours, 
            // then resolve it and throw in case of any error. Any error here would mean a dependency
            // issue. We want the application to crash in that case.
            if (serviceType.Assembly.FullName.StartsWith("BrighterOption")) // Obviously not a 100% fool-proof way. But it's not a big deal if some of our components are missed here. They'll get resolved below anyway. 
            {
                return container.GetInstance(serviceType);
            }

            // 2) If we're asked to resolve an abstract type or an interface and we have a concrete type configured
            // in our container for it, we're good and can return that. Otherwise, it means that
            // we don't know anything about the type we've been asked to resolve. In which case,
            // we must return null and WebAPI will take care of using a default implementation 
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
            // So make sure that you fully understand how both StructureMap and WebAPI work 
            // before going and changing the code above. This is no place for quick-and-dirty hacks.
            // There is very little code here but it's been very carefully written.
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.GetAllInstances(serviceType).Cast<object>();
        }

        public IDependencyScope BeginScope()
        {
            if (disposed)
                throw new ObjectDisposedException("WebApiDependencyResolver");

            return new WebApiDependencyResolver(container.GetNestedContainer(), true);
        }

        public void Dispose()
        {
            if (disposed)
                return;

            // We only want to dispose the nested DI containers that we created ourselves.
            // The top-level DI container that was provided to us on first instantiation
            // should be disposed of by the application itself on shutdown. We should not
            // mess with (or dispose!) objects that have been created by someone else.
            if (!isNestedResolver)
                return;

            container.Dispose();
            disposed = true;
        }
    }
}