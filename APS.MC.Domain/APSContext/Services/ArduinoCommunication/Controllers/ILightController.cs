using System.Threading.Tasks;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Queries.Lights;
using APS.MC.Domain.APSContext.ValueObjects;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Domain.APSContext.Services.ArduinoCommunication.Controllers
{
	public interface ILightController : INotifiable
	{
		Task<bool> Switch(SwitchLightQuery query);
		Task<bool> Read(PinPort pinPort);
	}
}