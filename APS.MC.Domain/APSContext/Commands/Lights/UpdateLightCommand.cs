using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Lights
{
	public class UpdateLightCommand : ICommand
	{
		public string Description { get; set; }
		public string PinPort { get; set; }

		public string LightId { get; private set; }

		public void SetLightId(string lightId) => LightId = lightId;
	}
}