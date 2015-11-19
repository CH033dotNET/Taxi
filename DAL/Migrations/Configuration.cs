namespace DAL.Migrations
{
    using Model;
    using Model.DB;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
	using Angela.Core;
	using Common.Enum.CarEnums;
	

	internal sealed class Configuration : DbMigrationsConfiguration<DAL.MainContext> 
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.MainContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
			Random random = new Random();
			var roles = new List<Role> 
			{
				new Role() { Name = "Driver", Description = "Driver" },
				new Role() { Name = "Operator", Description = "Operator" },
				new Role() { Name = "Client", Description = "Client" },
				new Role() { Name = "ReportViewer", Description = "Report Viewer" },
				new Role() { Name = "Administrator", Description = "Administrator" } 
			};


			roles.ForEach(s => context.Roles.AddOrUpdate(p => p.Name, s));

			context.SaveChanges();

			if (!context.Users.Any())
			{
				context.Users.AddOrUpdate(
                new User()
                {
                    UserName = "admin",
                    Password = "password",
                    Email = "admin@gmail.com",
                    RoleId = 5
                },
				new User()
				{
					UserName = "driver",
					Password = "password",
					Email = "driver@gmail.com",
					RoleId = 1
				},
                new User()
                {
                    UserName = "operator",
                    Password = "password",
                    Email = "operator@gmail.com",
                    RoleId = 2
                },
                new User()
                {
                    UserName = "client",
                    Password = "password",
                    Email = "client@gmail.com",
                    RoleId = 3
                },
                new User()
                {
                    UserName = "report",
                    Password = "password",
                    Email = "report@gmail.com",
                    RoleId = 4
                },
				new User()
                {
                    UserName = "Nickolas",
                    Password = "password",
                    Email = "coyotet@gmail.com",
                    RoleId = 1
                });
				var users = Angie.Configure<User>().Fill(x => x.UserName).AsTwitterHandle<User>().Fill(x => x.Email).AsEmailAddress<User>()
				.MakeList<User>(30).Select(x => new User { Password = "123456", RoleId = random.Next(1, 5), Email = x.Email, UserName = x.UserName });
				context.Users.AddRange(users);
				context.SaveChanges();
			}
			//Addres
			if (!context.Addresses.Any())
			{
				int counter = 1;
				var addres = Angie.Configure<UserAddress>().Fill(x => x.City).AsCity<UserAddress>().Fill(x => x.Street).AsAddress<UserAddress>()
				.MakeList<UserAddress>().Select(x => new UserAddress { City = x.City, Street = x.Street, Number = random.Next(1, 150).ToString(), Comment = x.Street, UserId = counter++ });
				context.Addresses.AddRange(addres);
				context.SaveChanges();
			}
			// District
			if (!context.Districts.Any())
			{
				var districts = Angie.Configure<District>().Fill(x => x.Name).AsCanadianProvince().MakeList<District>(6).Select(x => new District { Name = x.Name });
				context.Districts.AddRange(districts);
				context.SaveChanges();
			}
			//Person
			if (!context.Persons.Any())
			{
				int counter = 1;
				var persons = Angie.Configure<Person>().Fill(x => x.FirstName).AsFirstName<Person>().Fill(x => x.LastName).AsLastName<Person>().Fill(x => x.MiddleName).AsMusicArtistName<Person>()
					.Fill(x => x.Phone).AsPhoneNumber<Person>().MakeList<Person>()
					.Select(x => new Person { FirstName = x.FirstName, LastName = x.LastName, MiddleName = x.MiddleName, Phone = x.Phone, UserId = counter++ });
				context.Persons.AddRange(persons);
				context.SaveChanges();
			}
			//Tarif
			if (!context.Tarifes.Any())
			{
				Tarif[] tarifs = new Tarif[]{
				new Tarif() { Name = "Standart", MinimalPrice = 12.50M, OneMinuteCost = 1M, StartPrice = 5M, WaitingCost = 0.5M, IsStandart=true },
				new Tarif() { Name = "Region", MinimalPrice = 15.40M, OneMinuteCost = 1.4M, StartPrice = 8M, WaitingCost = 0.8M, IsIntercity = true },
				new Tarif() { Name ="Personal Driver", MinimalPrice = 40M, OneMinuteCost = 1.5M, StartPrice = 16M, WaitingCost =0.5M}
				};
				context.Tarifes.AddRange(tarifs);
				context.SaveChanges();
			}
			// Cars
			if (!context.Cars.Any())
			{
				Car[] cars = new Car[]
				{
					new Car() { CarName = "Lada", 
						CarNickName = "21", 
						CarNumber = "AK9265AK", 
						CarOccupation = 4, 
						CarPetrolConsumption = 2, 
						CarClass = CarClassEnum.General, 
						CarState = CarStateEnum.Working, 
						CarPetrolType = CarPetrolEnum.Regular92,
						OwnerId = context.Users.FirstOrDefault(x => x.RoleId == 1).Id,
						UserId = context.Users.FirstOrDefault(x => x.RoleId == 1).Id,
						CarManufactureDate = new DateTime(2012,03,12,00,00,00) },
					new Car() { CarName = "Volga", 
						CarNickName = "13", 
						CarNumber = "KX3456KX", 
						CarOccupation = 4, 
						CarPetrolConsumption = 4, 
						CarClass = CarClassEnum.General, 
						CarState = CarStateEnum.Working, 
						CarPetrolType = CarPetrolEnum.Regular92,
						OwnerId = context.Users.FirstOrDefault(x => x.RoleId == 1).Id,
						UserId = context.Users.FirstOrDefault(x => x.RoleId == 1).Id,
						CarManufactureDate = new DateTime(2008,07,22,00,00,00) },
					new Car() { CarName = "Audi", 
						CarNickName = "41", 
						CarNumber = "HO8712HO", 
						CarOccupation = 4, 
						CarPetrolConsumption = 8, 
						CarClass = CarClassEnum.General, 
						CarState = CarStateEnum.Working, 
						CarPetrolType = CarPetrolEnum.Premium95,
						OwnerId = context.Users.FirstOrDefault(x => x.RoleId == 1).Id,
						UserId = context.Users.FirstOrDefault(x => x.RoleId == 1).Id,
						CarManufactureDate = new DateTime(2011,11,05,00,00,00) },
					new Car() { CarName = "Mercedes Benz", 
						CarNickName = "134", 
						CarNumber = "IC6723IC", 
						CarOccupation = 4, 
						CarPetrolConsumption = 10, 
						CarClass = CarClassEnum.Econom, 
						CarState = CarStateEnum.Working, 
						CarPetrolType = CarPetrolEnum.Super98, 
						OwnerId = context.Users.FirstOrDefault(x => x.RoleId == 1).Id,
						UserId = context.Users.FirstOrDefault(x => x.RoleId == 1).Id,
						CarManufactureDate = new DateTime(2013,02,02,00,00,00) },
					new Car() { CarName = "Volkswagen", 
						CarNickName = "231", 
						CarNumber = "HH5634HH", 
						CarOccupation = 4, 
						CarPetrolConsumption = 3, 
						CarClass = CarClassEnum.General, 
						CarState = CarStateEnum.Working, 
						CarPetrolType = CarPetrolEnum.Regular92, 
						OwnerId = context.Users.FirstOrDefault(x => x.RoleId == 1).Id,
						UserId = context.Users.FirstOrDefault(x => x.RoleId == 1).Id,
						CarManufactureDate = new DateTime(2010,03,24,00,00,00) },
					new Car() { CarName = "Opel",
						CarNickName = "245", 
						CarNumber = "KO0000KO", 
						CarOccupation = 4, 
						CarPetrolConsumption = 7, 
						CarClass = CarClassEnum.Premium, 
						CarState = CarStateEnum.Working, 
						CarPetrolType = CarPetrolEnum.Regular92, 
						OwnerId = context.Users.FirstOrDefault(x => x.RoleId == 1).Id,
						UserId = context.Users.FirstOrDefault(x => x.RoleId == 1).Id,
						CarManufactureDate = new DateTime(2009,05,23,00,00,00) }
				};
				context.Cars.AddRange(cars);
				context.SaveChanges();
			}
			if (!context.Orders.Any())
			{
				var NewOrders = new List<Order>();
				for (int i = 0; i < 100; i++)
				{
					NewOrders.Add(new Order { OrderTime = DateTime.UtcNow, PeekPlace = string.Format("some place {0}", i), DropPlace = string.Format("some other place {0}", i), DriverId = 2, DistrictId = random.Next(1, 7), PersonId = random.Next(1, 25) });
				}
				context.Orders.AddRange(NewOrders);
				context.SaveChanges();
			}
        }
    }
}
