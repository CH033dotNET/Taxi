using System;
using System.Collections.Generic;
using System.Configuration;
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
				client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiUrl"]);
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
				client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiUrl"]);
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
