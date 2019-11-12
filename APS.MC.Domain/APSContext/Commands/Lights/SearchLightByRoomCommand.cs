using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Lights
{
	public class SearchLightByRoomCommand : ICommand
	{
		public string RoomId { get; set; }

		public void SetRoomId(string roomId) => RoomId = roomId;
	}
}