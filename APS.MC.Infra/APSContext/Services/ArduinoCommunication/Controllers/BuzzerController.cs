using System.Net.Http;
using System.Threading.Tasks;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Controllers;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Queries.Buzzers;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Responses;
using APS.MC.Domain.APSContext.ValueObjects;

namespace APS.MC.Infra.APSContext.Services.ArduinoCommunicationService.Controllers
{
	public class BuzzerController : ArduinoController, IBuzzerController
	{
		public BuzzerController(HttpClient client) : base(client) { }

		public async Task<bool> Switch(SwitchBuzzerQuery query)
		{
			string value = query.State ? "1" : "0";

			ARESTDefaultResponse response = await Send<ARESTDefaultResponse>(HttpMethod.Get, $"/digital/{query.PinPort.Value}/{value}");

			return response.Message == $"Pin D6 set to {value}";
		}

		public async Task<bool> Read(PinPort pinPort)
		{
			ARESTDefaultResponse response = await Send<ARESTDefaultResponse>(HttpMethod.Get, $"/ports?params={pinPort.Value}");

			return response.Return_Value != 0;
		}
	}
}