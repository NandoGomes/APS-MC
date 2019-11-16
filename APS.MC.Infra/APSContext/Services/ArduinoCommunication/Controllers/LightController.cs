using System.Net.Http;
using System.Threading.Tasks;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Controllers;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Queries.Lights;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Responses;

namespace APS.MC.Infra.APSContext.Services.ArduinoCommunicationService.Controllers
{
	public class LightController : ArduinoController, ILightController
	{
		public LightController(HttpClient client) : base(client) { }

		public Task<ARESTDefaultResponse> Switch(SwitchLightQuery query) => Send<ARESTDefaultResponse>(new HttpMethod("GET"), $"/digital/{query.PinPort.Value}/{(query.State ? 1 : 0)}");
	}
}