using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using APS.MC.Shared.APSShared.Notifications;
using Newtonsoft.Json;

namespace APS.MC.Infra.APSContext.Services.ArduinoCommunicationService.Controllers
{
	public abstract class ArduinoController : Notifiable
	{
		private readonly HttpClient _client;

		public ArduinoController(HttpClient client) => _client = client;

		protected async Task<TResult> Send<TResult>(HttpMethod method, string uri, object query = null, object body = null)
		{
			TResult result = default(TResult);

			HttpResponseMessage response = await _getResponse(method, uri, query, body);

			string content = response.Content.ReadAsStringAsync().Result;

			if (!string.IsNullOrEmpty(content))
				result = JsonConvert.DeserializeObject<TResult>(content);

			return result;
		}

		private async Task<HttpResponseMessage> _getResponse(HttpMethod method, string uri, object query, object body)
		{
			if (query != null)
			{
				NameValueCollection queryParser = HttpUtility.ParseQueryString(string.Empty);

				List<PropertyInfo> queryProperties = query.GetType().GetProperties().Where(queryProperty => queryProperty.GetMethod != null && queryProperty.GetValue(query) != null).ToList();

				foreach (PropertyInfo queryProperty in queryProperties)
					queryParser.Add(queryProperty.Name, queryProperty.GetValue(query).ToString());

				uri += $"?{queryParser.ToString()}";
			}

			HttpRequestMessage message = new HttpRequestMessage(method, uri);

			if (body != null)
				message.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

			return await _client.SendAsync(message);
		}
	}
}