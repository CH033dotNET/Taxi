using Model;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MainContext : DbContext
    {
        public MainContext()
            : base("dbTaxi")
        {
            this.Configuration.LazyLoadingEnabled = true;

        }

        public MainContext(string connString)
            : base(connString)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
		public DbSet<District> Districts { get; set; }
		public DbSet<Car> Cars { get; set; }
        public DbSet<UserAddress> Addresses { get; set; }
		public DbSet<Person> Persons { get; set; }
		public DbSet<VIPClient> VIPClients { get; set; }
        public DbSet<Localization> Localizations { get; set; }
		public DbSet<WorkshiftHistory> WorkshiftHistories { get; set; }
    }
}
