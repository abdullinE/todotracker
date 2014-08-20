using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using ToDoTracker.DataLayer;
using ToDoTracker.DataLayer.DataContext;
using ToDoTracker.Web.Helpers;

namespace ToDoTracker.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RegisterUnityContainer();
        }

        private void RegisterUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IToDoRepo, ToDoRepo>(new ContainerControlledLifetimeManager());
            container.RegisterType<ToDoDbContext, ToDoDbContext>(new ContainerControlledLifetimeManager());
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
