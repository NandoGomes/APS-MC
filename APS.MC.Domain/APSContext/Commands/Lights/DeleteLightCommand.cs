using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Lights
{
	public class DeleteLightCommand : ICommand
	{
		public string LightId { get; private set; }

		public void SetLightId(string lightId) => LightId = lightId;
	}
}