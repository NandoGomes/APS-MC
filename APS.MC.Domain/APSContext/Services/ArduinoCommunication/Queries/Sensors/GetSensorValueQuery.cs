using APS.MC.Domain.APSContext.Enums;
using APS.MC.Domain.APSContext.ValueObjects;

namespace APS.MC.Domain.APSContext.Services.ArduinoCommunication.Queries.Sensors
{
	public class GetSensorValueQuery
	{
		public GetSensorValueQuery(PinPort pinPort, ESensorType type)
		{
			PinPort = pinPort;
			Type = type;
		}

		public PinPort PinPort { get; private set; }
		public ESensorType Type { get; private set; }
	}
}