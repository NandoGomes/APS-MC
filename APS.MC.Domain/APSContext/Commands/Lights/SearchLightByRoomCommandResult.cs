using System.Collections.Generic;
using System.Net;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Notifications;
using MongoDB.Bson;

namespace APS.MC.Domain.APSContext.Commands.Lights
{
	public class SearchLightByRoomCommandResult : CommandResult
	{
		public SearchLightByRoomCommandResult() : base() { }
		public SearchLightByRoomCommandResult(HttpStatusCode code, IEnumerable<ObjectId> lights = null) : base(code)
		{
			Lights = lights;
		}
		public SearchLightByRoomCommandResult(HttpStatusCode code, IEnumerable<Notification> notifications) : base(code, notifications) { }

		public IEnumerable<ObjectId> Lights { get; private set; }
	}
}