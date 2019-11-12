using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Rooms
{
	public class CreateRoomCommand : ICommand
	{
		public string Description { get; set; }
	}
}