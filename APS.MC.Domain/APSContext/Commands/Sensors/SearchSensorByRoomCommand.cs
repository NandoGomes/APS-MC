using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Sensors
{
	public class SearchSensorByRoomCommand : ICommand
	{
		public string RoomId { get; set; }

		public void SetRoomId(string roomId) => RoomId = roomId;
	}
}