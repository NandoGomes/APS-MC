using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Rooms
{
	public class DeleteRoomCommand : ICommand
	{
		public string RoomId { get; private set; }

		public void SetRoomId(string roomId) => RoomId = roomId;
	}
}