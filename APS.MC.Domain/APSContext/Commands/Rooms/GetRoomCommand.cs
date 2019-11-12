using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Rooms
{
	public class GetRoomCommand : GetCommand
	{
		public string RoomId { get; private set; }

		public void SetRoomId(string roomId) => RoomId = roomId;
	}
}