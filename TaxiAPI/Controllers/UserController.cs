using BAL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model.DTO;
using System.Web;
using System.Text;

namespace TaxiAPI.Controllers
{
	public class UserController : ApiController
	{
		private IPersonManager personManager;

		public UserController(IPersonManager personManager)
		{
			this.personManager = personManager;
		}

		[HttpGet]
		[Route("api/User/GetPersonByUserId")]
		public HttpResponseMessage GetPersonByUserId(int data)
		{
			var person = personManager.GetPersonByUserId(data);
			if (person == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, person);
			}

			return Request.CreateResponse(HttpStatusCode.OK, person);
		}

		[HttpPost]
		[Route("api/User/EditPerson")]
		public HttpResponseMessage EditPerson(PersonDTO data)
		{
			var pers = personManager.EditPerson(data);
			if (pers == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, pers);
			}

			return Request.CreateResponse(HttpStatusCode.OK, pers);
		}

		[HttpPost]
		[Route("api/User/ImageDelete")]
		public HttpResponseMessage ImageDelete(PersonDTO data1, string data2)
		{
			var profileAvatar = "item_0_profile.jpg";
			ImageUpload imageUpload = new ImageUpload { Width = 200 };
			imageUpload.DeleteFile(data1.ImageName, profileAvatar, data2);
			return Request.CreateResponse(HttpStatusCode.OK, 1);
		}

		[HttpPost]
		[Route("api/User/RenameUploadFile")]
		public HttpResponseMessage RenameUploadFile(HttpPostedFileBase data)
		{
			ImageUpload imageUpload = new ImageUpload { Width = 200 };
			ImageResult imageResult = imageUpload.RenameUploadFile(data);
			StringBuilder result = new StringBuilder();
			if (imageResult.Success)
			{
				result.Append(imageResult.ImageName);
				return Request.CreateResponse(HttpStatusCode.BadRequest, result);
			}
			return Request.CreateResponse(HttpStatusCode.OK, result);
		}
	}
}
