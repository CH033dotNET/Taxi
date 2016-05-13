using DAL.Interface;
using DAL.Repositories;
using Model;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DAL
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		private MainContext context;

		#region Private Repositories

		private IGenericRepository<User> userRepo;
		private IGenericRepository<Role> roleRepo;
		private IGenericRepository<District> districtRepo;
		private IGenericRepository<Car> carInfo;
		private IGenericRepository<UserAddress> addressRepo;
		private IGenericRepository<Person> personRepo;
		private IGenericRepository<SupportMessage> supportRepo;

		private IGenericRepository<VIPClient> vipClientRepo;
		private IGenericRepository<Location> locationRepo;
		private IGenericRepository<WorkshiftHistory> workshiftHistoryRepo;

		private IGenericRepository<Tarif> tarifRepo;
		private IGenericRepository<Coordinates> coordinatesHistoryRepo;
		private IGenericRepository<Order> orderRepo;
		private IGenericRepository<WorkerStatus> workerStatusRepo;

		private IGenericRepository<Coordinate> districtCoordinatesRepo;

		private IGenericRepository<OrderEx> orderExRepo;

		private IGenericRepository<CoordinatesEx> coordinatesExRepo;
		private IGenericRepository<TariffEx> tariffExRepo;

		private IGenericRepository<AdditionallyRequirements> additionallyRequirementsRepo;
		private IGenericRepository<AddressFrom> addressFromRepo;
		private IGenericRepository<AddressTo> addressToRepo;
		private IGenericRepository<Feedback> feedbackRepo;
		#endregion

		public UnitOfWork()
		{
			context = new MainContext();

			userRepo = new GenericRepository<User>(context);
			roleRepo = new GenericRepository<Role>(context);
			districtRepo = new GenericRepository<District>(context);
			carInfo = new GenericRepository<Car>(context);
			addressRepo = new GenericRepository<UserAddress>(context);

			personRepo = new GenericRepository<Person>(context);

			vipClientRepo = new GenericRepository<VIPClient>(context);
			locationRepo = new GenericRepository<Location>(context);

			workshiftHistoryRepo = new GenericRepository<WorkshiftHistory>(context);

			tarifRepo = new GenericRepository<Tarif>(context);
			coordinatesHistoryRepo = new GenericRepository<Coordinates>(context);
			orderRepo = new GenericRepository<Order>(context);

			workerStatusRepo = new GenericRepository<WorkerStatus>(context);

			districtCoordinatesRepo = new GenericRepository<Coordinate>(context);
			supportRepo = new GenericRepository<SupportMessage>(context);

			orderExRepo = new GenericRepository<OrderEx>(context);

			coordinatesExRepo = new GenericRepository<CoordinatesEx>(context);
			tariffExRepo = new GenericRepository<TariffEx>(context);

			additionallyRequirementsRepo = new GenericRepository<AdditionallyRequirements>(context);
			addressFromRepo = new GenericRepository<AddressFrom>(context);
			addressToRepo = new GenericRepository<AddressTo>(context);
			feedbackRepo = new GenericRepository<Feedback>(context);
		}

		public void Save()
		{
			context.SaveChanges();
		}

		#region Repositories Getters

		public IGenericRepository<User> UserRepo
		{
			get
			{
				if (userRepo == null) userRepo = new GenericRepository<User>(context);
				return userRepo;
			}
		}

		public IGenericRepository<Role> RoleRepo
		{
			get
			{
				if (roleRepo == null) roleRepo = new GenericRepository<Role>(context);
				return roleRepo;
			}
		}

		public IGenericRepository<District> DistrictRepo
		{
			get
			{
				if (districtRepo == null) districtRepo = new GenericRepository<District>(context);
				return districtRepo;
			}
		}

		public IGenericRepository<Car> CarRepo
		{
			get
			{
				if (carInfo == null) carInfo = new GenericRepository<Car>(context);
				return carInfo;
			}
		}

		public IGenericRepository<UserAddress> AddressRepo
		{
			get
			{
				if (addressRepo == null) addressRepo = new GenericRepository<UserAddress>(context);
				return addressRepo;
			}
		}


		public IGenericRepository<Person> PersonRepo
		{
			get
			{
				if (personRepo == null) personRepo = new GenericRepository<Person>(context);
				return personRepo;

			}
		}
		public IGenericRepository<VIPClient> VIPClientRepo
		{
			get
			{
				if (vipClientRepo == null) vipClientRepo = new GenericRepository<VIPClient>(context);
				return vipClientRepo;

			}
		}
		public IGenericRepository<Location> LocationRepo
		{
			get
			{
				if (locationRepo == null) locationRepo = new GenericRepository<Location>(context);
				return locationRepo;
			}
		}
		public IGenericRepository<WorkshiftHistory> WorkshiftHistoryRepo
		{
			get
			{
				if (workshiftHistoryRepo == null) workshiftHistoryRepo = new GenericRepository<WorkshiftHistory>(context);
				return workshiftHistoryRepo;
			}
		}

		public IGenericRepository<Tarif> TarifRepo
		{
			get
			{
				if (tarifRepo == null) tarifRepo = new GenericRepository<Tarif>(context);
				return tarifRepo;
			}
		}

		public IGenericRepository<Coordinates> CoordinatesHistoryRepo
		{
			get
			{
				if (coordinatesHistoryRepo == null) coordinatesHistoryRepo = new GenericRepository<Coordinates>(context);
				return coordinatesHistoryRepo;
			}
		}

		public IGenericRepository<Order> OrderRepo
		{
			get
			{
				if (orderRepo == null) orderRepo = new GenericRepository<Order>(context);
				return orderRepo;
			}
		}

		public IGenericRepository<WorkerStatus> WorkerStatusRepo
		{
			get
			{
				if (workerStatusRepo == null) workerStatusRepo = new GenericRepository<WorkerStatus>(context);
				return workerStatusRepo;
			}
		}

		public IGenericRepository<Coordinate> DistrictCoordinatesRepo
		{
			get
			{
				if (districtCoordinatesRepo == null) districtCoordinatesRepo = new GenericRepository<Coordinate>(context);
				return districtCoordinatesRepo;
			}
		}

		public IGenericRepository<SupportMessage> SupportRepo
		{
			get
			{
				if (supportRepo == null) supportRepo = new GenericRepository<SupportMessage>(context);
				return supportRepo;
			}
		}

		public IGenericRepository<OrderEx> OrderExRepo
		{
			get
			{
				if (orderExRepo == null)
				{
					orderExRepo = new GenericRepository<OrderEx>(context);
				}
				return orderExRepo;
			}
		}

		public IGenericRepository<CoordinatesEx> CoordinatesExRepo
		{
			get
			{
				if (coordinatesExRepo == null)
				{
					coordinatesExRepo = new GenericRepository<CoordinatesEx>(context);
				}
				return coordinatesExRepo;
			}
		}

		public IGenericRepository<TariffEx> TariffExRepo
		{
			get
			{
				if (tariffExRepo == null)
				{
					tariffExRepo = new GenericRepository<TariffEx>(context);
				}
				return tariffExRepo;
			}
		}

		public IGenericRepository<AdditionallyRequirements> AdditionallyRequirementsRepo
		{
			get
			{
				if (additionallyRequirementsRepo == null)
				{
					additionallyRequirementsRepo = new GenericRepository<AdditionallyRequirements>(context);
				}
				return additionallyRequirementsRepo;
			}
		}

		public IGenericRepository<AddressFrom> AddressFromRepo
		{
			get
			{
				if (addressFromRepo == null)
				{
					addressFromRepo = new GenericRepository<AddressFrom>(context);
				}
				return addressFromRepo;
			}
		}

		public IGenericRepository<AddressTo> AddressToRepo
		{
			get
			{
				if (addressToRepo == null)
				{
					addressToRepo = new GenericRepository<AddressTo>(context);
				}
				return addressToRepo;
			}
		}

		public IGenericRepository<Feedback> FeedbackRepo
		{
			get
			{
				if (feedbackRepo == null)
				{
					feedbackRepo = new GenericRepository<Feedback>(context);
				}
				return feedbackRepo;
			}
		}
		#endregion

		#region Dispose
		// https://msdn.microsoft.com/ru-ru/library/system.idisposable(v=vs.110).aspx

		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					context.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}
