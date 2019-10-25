using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Sensors
{
	public class DeleteSensorCommand : ICommand
	{
		public string SensorId { get; private set; }

		public void SetSensorId(string sensorId) => SensorId = sensorId;
	}
}