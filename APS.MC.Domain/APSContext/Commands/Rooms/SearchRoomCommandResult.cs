using System.Collections.Generic;
using System.Net;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Notifications;
using MongoDB.Bson;

namespace APS.MC.Domain.APSContext.Commands.Rooms
{
	public class SearchRoomCommandResult : CommandResult
	{
		public SearchRoomCommandResult() : base() { }
		public SearchRoomCommandResult(HttpStatusCode code, IEnumerable<ObjectId> rooms = null) : base(code)
		{
			Rooms = rooms;
		}
		public SearchRoomCommandResult(HttpStatusCode code, IEnumerable<Notification> notifications) : base(code, notifications) { }

		public IEnumerable<ObjectId> Rooms { get; private set; }
	}
}