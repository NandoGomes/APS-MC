using APS.MC.Domain.APSContext.Enums;
using APS.MC.Domain.APSContext.ValueObjects;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Domain.APSContext.Services.ArduinoCommunication.Controllers
{
	public interface ISensorController : INotifiable
	{
		string GetValue(PinPort pinPort, ESensorType type);
	}
}