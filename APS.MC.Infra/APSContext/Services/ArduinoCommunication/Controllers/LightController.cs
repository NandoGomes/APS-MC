using System.Net.Http;
using System.Threading.Tasks;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Controllers;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Queries.Lights;

namespace APS.MC.Infra.APSContext.Services.ArduinoCommunicationService.Controllers
{
	public class LightController : ArduinoController, ILightController
	{
		public LightController(HttpClient client) : base(client) { }

		public Task<bool> Switch(SwichLightQuery query)
		{
			return Task.Factory.StartNew(() => true);
			// return Send<string>(new HttpMethod("PATCH"), "", query);
		}
	}
}