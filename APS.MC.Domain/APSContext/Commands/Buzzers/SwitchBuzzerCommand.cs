using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Buzzers
{
	public class SwitchBuzzerCommand : ICommand
	{
		public string BuzzerId { get; private set; }

		public void SetBuzzerId(string buzzerId) => BuzzerId = buzzerId;
	}
}