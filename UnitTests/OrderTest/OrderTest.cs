using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Model.DB;
using Model.DTO;

namespace UnitTests.OrderTest
{
	[TestClass]
	public class OrderTest
	{
		List<Coordinates> coordinates = new List<Coordinates>();
        List<Person> personData = new List<Person>();
        List<Order> dataСontainer = new List<Order>();
		public OrderTest()
        {
			#region Coordinates Inicialiized
            coordinates.AddRange(new Coordinates[]{ new Coordinates(){ UserId = 5,
             Latitude= 121.5, Longitude = 432.66}});
			#endregion
            #region Person Inicialized
            personData.AddRange(new Person[] { new Person() { Id = 7, FirstName = "Maks" },
            new Person(){ Id = 6, FirstName="Kostja" },new Person(){ Id = 5, FirstName="Nastja" },
            new Person(){ Id = 1, FirstName="Olja" },new Person(){ Id = 2, FirstName="Sanja" },
            new Person(){ Id = 3, FirstName="Valera"},new Person(){ Id = 10, FirstName="Ysov"},
            new Person(){ Id = 11, FirstName="Gora"},new Person(){ Id = 11, FirstName="Kolja"}});
            #endregion
            #region Order Inicialized
            dataСontainer.AddRange(new Order[]{ 
            new Order(){ Accuracy = 136.4, PeekPlace ="Lygovaja",
             DropPlace = "Golovnaja", PersonId = 7, IsConfirm = 1},
        new Order(){ Accuracy = 136.4,  PeekPlace ="Geroiv",
             DropPlace = "Kabuljanskoi", PersonId = 7, IsConfirm = 1},
            new Order(){ Accuracy = 136.4, PeekPlace ="Lygovaja",
             DropPlace = "Golovnaja", PersonId = 7, IsConfirm = 1},
        new Order(){ Id = 111, WaitingTime = "3", Accuracy = 136.4, DriverId = 5,  PeekPlace ="Geroiv",
             DropPlace = "Kabuljanskoi", PersonId = 6, IsConfirm = 4},
             new Order(){ Accuracy = 136.4, DriverId = 1,  PeekPlace ="Lygovaja",
             DropPlace = "Golovnaja", PersonId = 6, IsConfirm = 3},
        new Order(){ Accuracy = 136.4,  PeekPlace ="Geroiv",
             DropPlace = "Kabuljanskoi", PersonId = 1, IsConfirm = 3},
             new Order(){ Accuracy = 136.4, PeekPlace ="Lygovaja",
             DropPlace = "Golovnaja", PersonId = 2, IsConfirm = 3},
        new Order(){ Accuracy = 136.4, PeekPlace ="Geroiv",
             DropPlace = "Kabuljanskoi", PersonId = 3,IsConfirm = 3},
             new Order(){ Accuracy = 136.4,  PeekPlace ="Geroiv",
             DropPlace = "Kabuljanskoi", PersonId = 5, IsConfirm = 2},
            new Order(){ Accuracy = 136.4, DriverId = 1,  PeekPlace ="Lygovaja",
             DropPlace = "Golovnaja", PersonId = 4, IsConfirm = 3},
        new Order(){ Accuracy = 136.4, DriverId = 5,  PeekPlace ="Geroiv",
             DropPlace = "Kabuljanskoi", PersonId = 8, IsConfirm = 2},
             new Order(){ Accuracy = 136.4, DriverId = 1,  PeekPlace ="Lygovaja",
             DropPlace = "Golovnaja", PersonId = 9, IsConfirm = 3},
        new Order(){ Accuracy = 136.4, DriverId = 5,  PeekPlace ="Geroiv",
             DropPlace = "Kabuljanskoi", PersonId = 10, IsConfirm = 4},
             new Order(){ Accuracy = 136.4, DriverId = 1,  PeekPlace ="Lygovaja",
             DropPlace = "Golovnaja", PersonId = 11, IsConfirm = 1},
        new Order(){ Accuracy = 136.4, DriverId = 4,  PeekPlace ="Geroiv",
             DropPlace = "Kabuljanskoi", PersonId = 12,IsConfirm = 1}
            });
            #endregion
        }

        [TestMethod]
        public void TestGetTop10()
        {
            bool IsTopClientHasHighestOrdersAmount = false;
            var top10 = dataСontainer.GroupBy(x => x.PersonId).OrderByDescending(o => o.Select(y => y.PersonId).Count()).Take(10);
            if (top10.ElementAt(0).Count() >= top10.ElementAt(1).Count())
            {
                IsTopClientHasHighestOrdersAmount = true;
            }
            Assert.AreEqual(10, top10.Count(), "Make sure you have minimum 10 clients with orders");
            Assert.IsTrue(IsTopClientHasHighestOrdersAmount);
        }

        [TestMethod]
        public void TestOrdersData()
        {
            var newOrders = dataСontainer.Where(x => x.IsConfirm == 3 && x.DriverId == 0).ToList();
            var clients = personData.ToList();
            var operatorOrders = from O in newOrders
                                 join P in clients
                                 on O.PersonId equals P.Id
                                 select new { OrderId = O.Id, FirsName = P.FirstName, OrderTime = O.OrderTime, PeekPlace = O.PeekPlace, DropPlace = O.DropPlace };
            Assert.AreEqual(3, operatorOrders.Count());
        }

        [TestMethod]
        public void TestDriversRequest()
        {
            var confirmByOperatorsOrders = dataСontainer.Where(x => x.IsConfirm == 1 && x.DriverId != 0).ToList();
            var peoples = personData.ToList();
            var driverRequest = from O in confirmByOperatorsOrders
                                join P in peoples
                                on O.PersonId equals P.Id
                                select new { OrderId = O.Id, PeekPlace = O.PeekPlace, DropPlace = O.DropPlace, WaitingTime = O.WaitingTime, DriverId = O.DriverId };
            Assert.AreEqual(2, driverRequest.Count());
        }

        [TestMethod]
        public void TestGetWaitingOrders()
        {
            var orders = dataСontainer.Where(x => x.IsConfirm == 1 && x.DriverId == 0).ToList();
            var peoples = personData.ToList();

            var operatorOrders = from O in orders
                                 join P in peoples
                                 on O.PersonId equals P.Id
                                 select new { OrderId = O.Id, FirsName = P.FirstName, OrderTime = O.OrderTime, PeekPlace = O.PeekPlace, DropPlace = O.DropPlace };
            Assert.AreEqual(3, operatorOrders.Count());
        }
         [TestMethod]
        public void TestGetOrderedTaxi()
        {
            var particularOrder = dataСontainer.Where(x=>x.Id == 111).Select(obj=>obj).First();
            ClientOrderedDTO currentOrder = null;
            if (particularOrder.IsConfirm == 4)
            {
                var driverCoordinates = coordinates.Where(x => x.UserId == particularOrder.DriverId).FirstOrDefault();
                if (driverCoordinates != null)
                {
                     currentOrder = new ClientOrderedDTO()
                        {
                            IsConfirm = particularOrder.IsConfirm,
                            Latitude = driverCoordinates.Latitude,
                            Longitude = driverCoordinates.Longitude,
                            WaitingTime = particularOrder.WaitingTime
                        };
                   
                }
                Assert.IsNotNull(driverCoordinates);
            }
            Assert.IsNotNull(currentOrder);
        }

	}
}
