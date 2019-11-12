using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Buzzers
{
	public class UpdateBuzzerCommand : ICommand
	{
		public string Description { get; set; }
		public string PinPort { get; set; }

		public string BuzzerId { get; private set; }

		public void SetBuzzerId(string buzzerId) => BuzzerId = buzzerId;
	}
}