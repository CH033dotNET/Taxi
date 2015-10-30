using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Common.Helpers
{
	public class RequestHelper
	{
		private static RestClient client;

		public RequestHelper(string apiUrl)
		{
			client = new RestClient();

			if (apiUrl != null)
			{
				client.BaseUrl = new Uri(apiUrl);
			}
			else
			{
				throw new Exception("Web config does not have API Url");
			}
		}
		//Get request
		public IRestResponse<T> Get<T>(string controller, string method) where T : new()
		{
			var request = new RestRequest(string.Format("{0}/{1}", controller, method), Method.GET);
			var result = client.Execute<T>(request);

			return result;
		}

		// Get request
		//api/controller/id
		public IRestResponse<T> GetById<T>(string controller, string method, int id) where T : new()
		{
			var request = new RestRequest(string.Format("{0}/{1}/{2}", controller, method, id), Method.GET);
			request.AddParameter("id", id);

			/*if (HttpContext.Current.User.Identity.IsAuthenticated) {
				request.AddHeader("Authorization", getToken());
			}*/
			var result = client.Execute<T>(request);

			return result;
		}

		public IRestResponse<List<T>> GetAll<T>(string controller, string method) where T : new()
		{
			var request = new RestRequest(string.Format("{0}/{1}", controller, method), Method.GET);

			var result = client.Execute<List<T>>(request);

			return result;
		}

		//Put request
		// api/controller/id
		public IRestResponse<T> PutById<T>(string controller, string method, int id) where T : new()
		{
			var request = new RestRequest(string.Format("{0}/{1}/{2}", controller, method, id), Method.PUT);
			request.AddParameter("id", id);

			var result = client.Execute<T>(request);

			return result;
		}

		public IRestResponse<T> PutObject<T>(string controller, string method, T modelObj) where T : new()
		{
			var request = new RestRequest(string.Format("{0}/{1}", controller, method), Method.PUT);
			//request.AddParameter("modelObj", modelObj);
			request.AddBody(modelObj);

			var result = client.Execute<T>(request);

			return result;
		}

		// Post request
		// api/controller/
		//public IRestResponse<T> Post<T>(string controller, string method) where T : new()
		//{
		//	var request = new RestRequest(string.Format("{0}/{1}", controller, method), Method.POST);
		//	var result = client.Execute<T>(request);

		//	return result;
		//}

		public IRestResponse<T> PostObject<T>(string controller, string method, T modelObj) where T : new()
		{
			var request = new RestRequest(string.Format("{0}/{1}", controller, method), Method.POST);
			request.AddBody(modelObj);
			var result = client.Execute<T>(request);

			return result;
		}

		// delete request
		//api/controller/id
		public IRestResponse<T> DeleteById<T>(string controller, string method, int id) where T : new()
		{
			var request = new RestRequest(string.Format("{0}/{1}/{2}", controller, method, id), Method.DELETE);
			request.AddParameter("id", id);

			var result = client.Execute<T>(request);

			return result;
		}
	}
}
