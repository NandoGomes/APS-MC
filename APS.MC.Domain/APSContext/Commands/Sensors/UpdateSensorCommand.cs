using APS.MC.Domain.APSContext.Enums;
using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Sensors
{
	public class UpdateSensorCommand : ICommand
	{
		public string Description { get; set; }
		public string PinPort { get; set; }
		public ESensorType Type { get; set; }

		public string SensorId { get; private set; }

		public void SetSensorId(string sensorId) => SensorId = sensorId;
	}
}