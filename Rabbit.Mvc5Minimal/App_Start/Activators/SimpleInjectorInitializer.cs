using Rabbit.IOC;
using Rabbit.Mvc5Minimal.App_Start.Activators;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Packaging;
using System.Linq;
using System.Web.Mvc;
using WebActivator;

[assembly: PostApplicationStartMethod(typeof(SimpleInjectorInitializer), "Initialize")]

namespace Rabbit.Mvc5Minimal.App_Start.Activators
{
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            InitializeContainer(container);

            container.RegisterMvcControllers(typeof(SimpleInjectorInitializer).Assembly);

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            ModuleHelper.GetModuleTypes(typeof(SimpleInjectorInitializer).Assembly)
                .CreateModules()
                .ToList()
                .ForEach(x => ((IPackage)x).RegisterServices(container));
        }
    }
}