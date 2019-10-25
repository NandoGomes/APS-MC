using System.Net.Http;
using System.Threading.Tasks;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Controllers;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Queries.Sensors;

namespace APS.MC.Infra.CommonContext.Services.ArduinoCommunicationService.Controllers
{
	public class SensorController : ArduinoController, ISensorController
	{
		public SensorController(HttpClient client) : base(client) { }

		public Task<string> GetValue(GetSensorValueQuery query) => Send<string>(HttpMethod.Get, "", query);
	}
}