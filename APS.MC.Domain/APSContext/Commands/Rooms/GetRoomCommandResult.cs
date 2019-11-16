using System.Collections.Generic;
using System.Net;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Notifications;
using MongoDB.Bson;

namespace APS.MC.Domain.APSContext.Commands.Rooms
{
	public class GetRoomCommandResult : CommandResult
	{
		public GetRoomCommandResult() : base() { }
		public GetRoomCommandResult(HttpStatusCode code) : base(code) { }
		public GetRoomCommandResult(HttpStatusCode code, IEnumerable<Notification> notifications) : base(code, notifications) { }

		public ObjectId? Id { get; set; }
		public string Description { get; set; }
	}
}