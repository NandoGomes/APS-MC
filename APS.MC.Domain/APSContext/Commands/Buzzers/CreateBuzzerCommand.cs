using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Buzzers
{
	public class CreateBuzzerCommand : ICommand
	{
		public string Description { get; set; }
		public string PinPort { get; set; }
		public string RoomId { get; set; }
	}
}