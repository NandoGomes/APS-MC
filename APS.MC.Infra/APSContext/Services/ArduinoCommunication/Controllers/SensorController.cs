using System.Net.Http;
using System.Threading.Tasks;
using APS.MC.Domain.APSContext.Enums;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Controllers;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Queries.Sensors;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Responses;

namespace APS.MC.Infra.APSContext.Services.ArduinoCommunicationService.Controllers
{
	public class SensorController : ArduinoController, ISensorController
	{
		public SensorController(HttpClient client) : base(client) { }

		public async Task<decimal> GetValue(GetSensorValueQuery query)
		{
			string url = "/sensors/";

			switch (query.Type)
			{
				case ESensorType.Humidity:
					url += "humidity";

					break;

				case ESensorType.Temperature:
					url += "temperature";

					break;
			}

			ARESTDefaultResponse response = await Send<ARESTDefaultResponse>(HttpMethod.Get, $"{url}/{query.PinPort}");

			return response.Return_Value / 100;
		}
	}
}