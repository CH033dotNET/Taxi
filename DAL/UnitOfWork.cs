using DAL.Interface;
using DAL.Repositories;
using Model;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		private IGenericRepository<VIPClient> vipClientRepo;
        private IGenericRepository<Localization> localizationRepo;

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
            localizationRepo = new GenericRepository<Localization>(context);
            
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
        public IGenericRepository<Localization> LocalizationRepo
        {
            get
            {
                if (localizationRepo == null) localizationRepo = new GenericRepository<Localization>(context);
                return localizationRepo;
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
