using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Sensors
{
	public class GetSensorCommand : GetCommand
	{
		public string SensorId { get; private set; }

		public void SetSensorId(string sensorId) => SensorId = sensorId;
	}
}