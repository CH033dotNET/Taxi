using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxiApi.Controllers;
using System.Text;
using Moq;
using BAL.Manager;
using System.Collections.Generic;
using Model;
using Model.DTO;
using DAL.Interface;
using System.Linq;
using System.Web.Http.Results;
using Common;

namespace Tests.UnitTestTaxiApi
{
	[TestClass]
	public class UserControllerTest
	{
		[TestMethod]
		public void LoginTest()
		{
			//arrange
			var uof = new Mock<IUnitOfWork>();
			uof.Setup(m => m.UserRepo.All).Returns((new List<User>() { new User() { UserName = "admin", Password = "password" } }).AsQueryable());
			var manager = new UserManager(uof.Object);
			UserController controller = new UserController(manager);
			AutoMapperConfig.Configure();

			//act
			var result = controller.Login("admidn", "password") as JsonResult<UserDTO>;
			var correctResult = controller.Login("admin", "password") as JsonResult<UserDTO>;

			//assert
			Assert.IsNull(result.Content);
			Assert.AreEqual("admin",  correctResult.Content.UserName);
		}
	}
}
