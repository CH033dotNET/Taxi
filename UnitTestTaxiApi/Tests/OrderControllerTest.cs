using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BAL.Manager;
using DAL.Interface;
using TaxiApi.Controllers;
using Common;
using System.Collections.Generic;
using System.Web.Http.Results;
using Model;
using Model.DTO;
using System.Linq;
using Model.DB;
using BAL.Interfaces;
using Common.Enum;

namespace Tests.UnitTestTaxiApi
{
	[TestClass]
	public class OrderControllerTest
	{
		[TestMethod]
		public void CreateOrderTest()
		{
			//arrange
			var uof = new Mock<IUnitOfWork>();
			uof.Setup(m => m.OrderExRepo.All).Returns((new List<OrderEx>() { new OrderEx() { } }).AsQueryable());
			var manager = new OrderManagerEx(uof.Object);
			var controller = new OrderController(manager);
			AutoMapperConfig.Configure();

			//act
			var result = controller.CreateOrder("Golovna 202") as JsonResult<OrderExDTO>;

			//assert
			Assert.IsTrue("Golovna 202" == result.Content.FullAddressFrom && result.Content.Status == OrderStatusEnum.NotApproved);
		}

	}
}
