using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Sensors
{
	public class ReadSensorCommand : ICommand
	{
		public string SensorId { get; private set; }

		private void SetSensorId(string sensorId) => SensorId = sensorId;
	}
}