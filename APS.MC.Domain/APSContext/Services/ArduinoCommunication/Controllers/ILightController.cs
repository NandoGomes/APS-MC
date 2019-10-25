using System.Threading.Tasks;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Queries.Lights;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Domain.APSContext.Services.ArduinoCommunication.Controllers
{
	public interface ILightController : INotifiable
	{
		Task<bool> Switch(SwichLightQuery query);
	}
}