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
		Random random = new Random();

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
			var roles = new List<Role> 
			{
				new Role() { Name = "Driver", Description = "Driver" },
				new Role() { Name = "Operator", Description = "Operator" },
				new Role() { Name = "Client", Description = "Client" },
				new Role() { Name = "ReportViewer", Description = "Report Viewer" },
				new Role() { Name = "Administrator", Description = "Administrator" },
				new Role() { Name = "Supporter", Description = "Support Service"},
				new Role() { Name = "FreeDriver", Description = "Free Driver" }
			};

			roles.ForEach(s => context.Roles.AddOrUpdate(p => p.Name, s));
			context.SaveChanges();

			var users = new List<User>
			{
				new User()
				{
					UserName = "admin",
					Password = "password",
					Email = "admin@gmail.com",
					RoleId = 5,
					Lang = "en-us",
					RegistrationDate = DateTime.Now
				},
				new User()
				{
					UserName = "driver",
					Password = "password",
					Email = "driver@gmail.com",
					RoleId = 1,
					Lang = "en-us",
					RegistrationDate = DateTime.Now
				},
				new User()
				{
					UserName = "operator",
					Password = "password",
					Email = "operator@gmail.com",
					RoleId = 2,
					Lang = "en-us",
					RegistrationDate = DateTime.Now
				},
				new User()
				{
					UserName = "client",
					Password = "password",
					Email = "client@gmail.com",
					RoleId = 3,
					Lang = "en-us",
					RegistrationDate = DateTime.Now,
					Bonus = 10.0f
				},
				new User()
				{
					UserName = "report",
					Password = "password",
					Email = "report@gmail.com",
					RoleId = 4,
					Lang = "en-us",
					RegistrationDate = DateTime.Now
				},
				new User()
				{
					UserName = "Nickolas",
					Password = "password",
					Email = "coyotet@gmail.com",
					RoleId = 1,
					Lang = "en-us",
					RegistrationDate = DateTime.Now
				},
				new User()
				{
					UserName = "support",
					Password = "password",
					Email = "support@gmail.com",
					RoleId = 6,
					Lang = "en-us",
					RegistrationDate = DateTime.Now
				},
				new User()
				{
					UserName = "freedriver",
					Password = "password",
					Email = "freedriver@gmail.com",
					RoleId = 7,
					Lang = "en-us",
					RegistrationDate = DateTime.Now
				}
			};

			users.ForEach(u => context.Users.AddOrUpdate(p => p.UserName, u));

			if (!context.Users.Any())
			{
				var rundomUsers = Angie.Configure<User>().Fill(x => x.UserName).AsTwitterHandle<User>().Fill(x => x.Email).AsEmailAddress<User>()
				.MakeList<User>(30).Select(x => new User { Password = "123456", RoleId = random.Next(1, 5), Email = x.Email, UserName = x.UserName, Lang = "en-us", RegistrationDate = DateTime.Now, Bonus = random.Next(3,50)});
				context.Users.AddRange(rundomUsers);
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
			//Tariff
			if (!context.TariffsEx.Any())
			{
				TariffEx tariff = new TariffEx() {
						Name = "Standart",
						Description = "Standart tariff",

						PriceOutCity = 5,
						PriceInCity = 2.5M,
						PriceClientCarOption = 5,

						PriceRegularCar = 0,
						PriceMinivanCar = 15,
						PriceLuxCar = 40,

						PriceCourierOption = 15,
						PricePassengerSmokerOption = 20,
						PricePlateOption = 5,
						PricePreOrder = 10,
						PriceSpeakEnglishOption = 20
					};
				context.TariffsEx.Add(tariff);
				context.SaveChanges();
			}
			//News
			if (!context.News.Any())
			{
				News article1 = new News()
				{
					Title = "Lorem ipsum",
					Article = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
					CreatedTime = DateTime .UtcNow
				};
				News article2 = new News()
				{
					Title = "Lorem ipsum",
					Article = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
					CreatedTime = DateTime.UtcNow
				};
				News article3 = new News()
				{
					Title = "Lorem ipsum",
					Article = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
					CreatedTime = DateTime.UtcNow
				};
				News article4 = new News()
				{
					Title = "Lorem ipsum",
					Article = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
					CreatedTime = DateTime.UtcNow
				};
				context.News.Add(article1);
				context.News.Add(article2);
				context.News.Add(article3);
				context.News.Add(article4);
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
						CarClass = CarClassEnum.Normal, 
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
						CarClass = CarClassEnum.Normal, 
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
						CarClass = CarClassEnum.Normal, 
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
						CarClass = CarClassEnum.Normal, 
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
						CarClass = CarClassEnum.Universal, 
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
						CarClass = CarClassEnum.Lux, 
						CarState = CarStateEnum.Working, 
						CarPetrolType = CarPetrolEnum.Regular92, 
						OwnerId = context.Users.FirstOrDefault(x => x.RoleId == 1).Id,
						UserId = context.Users.FirstOrDefault(x => x.RoleId == 1).Id,
						CarManufactureDate = new DateTime(2009,05,23,00,00,00) }
				};
				context.Cars.AddRange(cars);
				context.SaveChanges();
			}
			if (!context.OrderdsEx.Any())
			{
				var r = context.Users.Where(x => x.RoleId == (int)Common.Enum.AvailableRoles.Driver || x.RoleId == (int)Common.Enum.AvailableRoles.FreeDriver).Select(x => x.Id).ToArray();
				var NewOrders = new List<OrderEx>();
				int[] orderStatuses = new[] {0,1,2,4,5};

				for (int i = 0; i < 30; i++)
				{
					var date = DateTime.Now.AddDays(-random.Next(0, 30));
					//set userid null
					bool user = (random.Next(0, 6) > 2) ? true : false;
					int orderStatus = orderStatuses[random.Next(0, orderStatuses.Length)];

					NewOrders.Add(new OrderEx()
						{
							AdditionallyRequirements = (user==true)? getAdditional(): null,
							AddressFrom = new AddressFrom{
								Address = GetRandomAddress(random.Next(1, 5))
							},
							AddressesTo = (user==true)?getDestinationAddress(random.Next(1, 6)):null,
							Status = (Common.Enum.OrderStatusEnum) orderStatus,
							DriverId = (orderStatus == 0 || orderStatus == 1 ) ? null: (int?) 2,
							CarId = (orderStatus == 0 || orderStatus == 1) ? null : (int?)3,
							Route = (user == true) ? true : false,
							UserId = (user==true)? (int?) 4 : null,
							Perquisite = (user==true)?random.Next(5,20) : 0,
							Name = (user==false) ? null : "User"+i,
							OrderTime = date,
							Price = random.Next(5,70),
							WaitingTime = random.Next(2,10)
						});
				}
				// set 1 in-progress order
				NewOrders.Add(new OrderEx()
				{
					AdditionallyRequirements = getAdditional(),
					AddressFrom = new AddressFrom
					{
						Address = GetRandomAddress(1)
					},
					AddressesTo = getDestinationAddress(5),
					Status = Common.Enum.OrderStatusEnum.Confirmed,
					DriverId = 2,
					CarId = 3,
					Route = (random.Next(0, 2) == 1) ? true : false,
					UserId = 4,
					Perquisite = random.Next(5, 20),
					Name = "User" + 100,
					OrderTime = DateTime.Now,
					Price = random.Next(5, 50),
					WaitingTime = random.Next(2, 10)
				});
				context.OrderdsEx.AddRange(NewOrders);
				context.SaveChanges();
			}
        }
		AdditionallyRequirements getAdditional()
		{
			Array carEnums = Enum.GetValues(typeof(Common.Enum.CarEnums.CarClassEnum));
			return new AdditionallyRequirements()
			{
				Passengers = random.Next(1, 9),
				Car = (Common.Enum.CarEnums.CarClassEnum)carEnums.GetValue(random.Next(carEnums.Length)),
				Urgently = (random.Next(0, 2) == 1) ? true : false,
				Courier = (random.Next(0, 2) == 1) ? true : false,
				WithPlate = (random.Next(0, 2) == 1) ? true : false,
				MyCar = (random.Next(0, 2) == 1) ? true : false,
				Pets = (random.Next(0, 2) == 1) ? true : false,
				Bag = (random.Next(0, 2) == 1) ? true : false,
				Conditioner = (random.Next(0, 2) == 1) ? true : false,
				NoSmoking = (random.Next(0, 2) == 1) ? true : false,
				Smoking = (random.Next(0, 2) == 1) ? true : false,
				English = (random.Next(0, 2) == 1) ? true : false,
				Check = (random.Next(0, 2) == 1) ? true : false,
			};
		}
		List<AddressTo> getDestinationAddress(int count)
		{
			List<AddressTo> addressesTo = new List<AddressTo>(); 

			for (int i = 0; i < count; i++)
			{
				addressesTo.Add(new AddressTo{
					Address = GetRandomAddress(random.Next(1, 5))
				});
			}
			return addressesTo;
		}
		string GetRandomAddress(int rand)
		{

			string newAddress = "";
			switch (rand)
			{
				case 1: newAddress = "������� ������, " + random.Next(3, 250) + ", ��������, ����������� �������, ������"; break;
				case 2: newAddress = "������ ϳ�������������, " + random.Next(3, 30) + ", ��������, ����������� �������, �������"; break;
				case 3: newAddress = "������ ����������, " + random.Next(1, 115) + ", ��������, ����������� �������, �������"; break;
				case 4: newAddress = "������ ����� ������� " + random.Next(1, 200) + ", ��������, ����������� �������, �������"; break;
			}

			return newAddress;
		}
    }
}
