using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Rooms
{
	public class UpdateRoomCommand : ICommand
	{
		public string Description { get; set; }

		public string RoomId { get; private set; }

		public void SetRoomId(string roomId) => RoomId = roomId;
	}
}