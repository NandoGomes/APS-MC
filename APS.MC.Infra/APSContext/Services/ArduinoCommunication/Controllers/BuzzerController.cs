using System.Net.Http;
using System.Threading.Tasks;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Controllers;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Queries.Buzzers;

namespace APS.MC.Infra.APSContext.Services.ArduinoCommunicationService.Controllers
{
	public class BuzzerController : ArduinoController, IBuzzerController
	{
		public BuzzerController(HttpClient client) : base(client) { }

		public Task<bool> Switch(SwitchBuzzerQuery query)
		{
			return Task.Factory.StartNew(() => true);
			//return Send<bool>(new HttpMethod("PATCH"), "", query);
		}
	}
}