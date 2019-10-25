using APS.MC.Domain.APSContext.Enums;
using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Sensors
{
	public class CreateSensorCommand : ICommand
	{
		public string Description { get; set; }
		public string PinPort { get; set; }
		public ESensorType Type { get; set; }
	}
}