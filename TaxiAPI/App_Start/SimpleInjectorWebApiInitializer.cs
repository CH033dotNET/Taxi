[assembly: WebActivator.PostApplicationStartMethod(typeof(TaxiAPI.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace TaxiAPI.App_Start
{
    using System.Web.Http;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
	using DAL.Interface;
	using DAL;
	using BAL.Manager;
	using BAL.Interfaces;
    
    public static class SimpleInjectorWebApiInitializer
    {
        /// <summary>Initialize the container and register it as Web API Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            
            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
       
            container.Verify();
            
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
     
        private static void InitializeContainer(Container container)
        {
			container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);
			container.Register<IAddressManager, AddressManager>();
			container.Register<ICarManager, CarManager>();
			container.Register<ICoordinatesManager, CoordinatesManager>();
			container.Register<IDistrictManager, DistrictManager>();
			container.Register<IDriverManager, DriverManager>();
			container.Register<ILocationManager, LocationManager>();
			container.Register<IOrderManager, OrderManager>();
			container.Register<IPersonManager, PersonManager>();
			container.Register<ITarifManager, TarifManager>();
			container.Register<IUserManager, UserManager>();
			container.Register<IWorkerStatusManager, WorkerStatusManager>();
			//container.Register<IDriverLocationHelper, DriverLocationHelper>();
            // For instance:
            // container.Register<IUserRepository, SqlUserRepository>(Lifestyle.Scoped);
        }
    }
}