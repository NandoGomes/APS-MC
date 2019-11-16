using System.Threading.Tasks;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Queries.Buzzers;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Responses;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Domain.APSContext.Services.ArduinoCommunication.Controllers
{
	public interface IBuzzerController : INotifiable
	{
		Task<ARESTDefaultResponse> Switch(SwitchBuzzerQuery query);
	}
}