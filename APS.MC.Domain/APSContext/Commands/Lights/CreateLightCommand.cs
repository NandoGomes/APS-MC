using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Lights
{
	public class CreateLightCommand : ICommand
	{
		public string Description { get; set; }
		public string PinPort { get; set; }
	}
}