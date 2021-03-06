﻿using Model;
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
			: base("Taxi")
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
		public DbSet<Location> Locations { get; set; }
		public DbSet<WorkshiftHistory> WorkshiftHistories { get; set; }
		public DbSet<Tarif> Tarifes { get; set; }
		public DbSet<Coordinates> CoordinatesHistory { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<WorkerStatus> WorkersStatuses { get; set; }
		public DbSet<SupportMessage> SupportChat { get; set; }
		public DbSet<Coordinate> DistrictCoordiantes { get; set; }
		public DbSet<OrderEx> OrderdsEx { get; set; }
		public DbSet<CoordinatesEx> CoordinatesEx { get; set; }
		public DbSet<TariffEx> TariffsEx { get; set; }
		public DbSet<AdditionallyRequirements> AdditionallyRequirements { get; set; }
		public DbSet<AddressFrom> AddressesFrom { get; set; }
		public DbSet<AddressTo> AddressesTo { get; set; }
		public DbSet<Feedback> FeedBacks { get; set; }
		public DbSet<News> News { get; set; }
	}
}
