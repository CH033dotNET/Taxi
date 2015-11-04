﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;

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

        public IRestResponse<T> Get<T,T2>(string controller, string method, T2 data) where T : new()
        {
            var request = new RestRequest(string.Format("{0}/{1}", controller, method), Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(data);

            var result = client.Execute<T>(request);

            return result;
        }

		// Get request
		//api/controller/id
		public IRestResponse<T> GetById<T>(string controller, string method, int id) where T : new()
		{
            var request = new RestRequest(string.Format("{0}/{1}?id={2}", controller, method, id), Method.GET);
			//request.AddParameter("id", id);

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
			var request = new RestRequest(string.Format("{0}/{1}", controller, method), Method.POST);
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

        
        public IRestResponse<T1> Get<T1,T2,T3>(string controller, string method, T2 param1, T3 param2) where T1 : new()
        {
            var request = new RestRequest(string.Format("{0}/{1}?param1={2}&param2={3}", controller, method, param1, param2), Method.GET);

            /*if (HttpContext.Current.User.Identity.IsAuthenticated) {
                request.AddHeader("Authorization", getToken());
            }*/
            var result = client.Execute<T1>(request);

            return result;
        }

        public IRestResponse<T> PostJ<T,T2>(string controller, string method, T2 item) where T : new()
        {
            var request = new RestRequest(string.Format("{0}/{1}", controller, method), Method.POST);
            var json = JsonConvert.SerializeObject(item);
            request.AddParameter("text/json", json, ParameterType.RequestBody);

            var result = client.Execute<T>(request);
            return result;
        }

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
