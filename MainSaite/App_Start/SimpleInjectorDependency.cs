using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BAL;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using BAL.Manager;
using System.Web.Mvc;
using DAL.Interface;
using DAL;
using SimpleInjector.Integration.Web;
using MainSaite.Helpers;
using BAL.Interfaces;

namespace MainSaite.App_Start
{
	public class SimpleInjectorDependency
	{

		public static void RegistrationContainers()
		{
			var container = new Container();
			container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
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
			container.Register<IDriverLocationHelper, DriverLocationHelper>();
			container.Register<IWorkerStatusManager, WorkerStatusManager>();
			container.Register<ISupportManager, SupportManager>();
			container.Register<IOrderManagerEx, OrderManagerEx>();
			container.Register<IDriverExManager, DriverExManager>();
			container.Verify();

			DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
		
		}
	

	}
}