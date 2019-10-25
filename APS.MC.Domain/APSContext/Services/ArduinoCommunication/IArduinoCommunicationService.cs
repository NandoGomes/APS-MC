using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Controllers;

namespace APS.MC.Domain.APSContext.Services.ArduinoCommunication
{
	public interface IArduinoCommunicationService
	{
		ISensorController Sensors { get; }
		ILightController Lights { get; }
	}
}