using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WindowsFormsApiApplication.Tools
{
	public static class RequestHelper
	{
		public static async Task<TResult> Get<TResult>(string url)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://localhost:40490/");
				HttpResponseMessage response = await client.GetAsync(url);
				if (response.IsSuccessStatusCode)
				{
					var user = await response.Content.ReadAsStringAsync();
					var seriazer = new JavaScriptSerializer();
					return seriazer.Deserialize<TResult>(user);
				}
				return default(TResult);
			}
		}

		public static async Task<TResult> Post<TResult>(string url, string data)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://localhost:40490/");
				HttpResponseMessage response = await client.PostAsync(url, new StringContent(data));
				if (response.IsSuccessStatusCode)
				{
					var user = await response.Content.ReadAsStringAsync();
					var seriazer = new JavaScriptSerializer();
					return seriazer.Deserialize<TResult>(user);
				}
				return default(TResult);
			}
		}
	}
}
