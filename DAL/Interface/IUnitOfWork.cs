using Model;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
	public interface IUnitOfWork
	{
		IGenericRepository<User> UserRepo { get; }
		IGenericRepository<Role> RoleRepo { get; }
		IGenericRepository<District> DistrictRepo { get; }
		IGenericRepository<Car> CarRepo { get; }
		IGenericRepository<UserAddress> AddressRepo { get; }
		IGenericRepository<Person> PersonRepo { get; }
		IGenericRepository<VIPClient> VIPClientRepo { get; }
		IGenericRepository<Location> LocationRepo { get; }
		IGenericRepository<WorkshiftHistory> WorkshiftHistoryRepo { get; }
        IGenericRepository<Tarif> TarifRepo { get; }

		void Dispose();
		void Save();
	}
}
