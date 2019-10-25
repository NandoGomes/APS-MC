using System.Threading.Tasks;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Queries.Sensors;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Domain.APSContext.Services.ArduinoCommunication.Controllers
{
	public interface ISensorController : INotifiable
	{
		Task<string> GetValue(GetSensorValueQuery query);
	}
}