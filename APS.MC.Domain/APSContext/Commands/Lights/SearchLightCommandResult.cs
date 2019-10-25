using System.Collections.Generic;
using System.Net;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Notifications;
using MongoDB.Bson;

namespace APS.MC.Domain.APSContext.Commands.Lights
{
	public class SearchLightCommandResult : CommandResult
	{
		public SearchLightCommandResult() : base() { }
		public SearchLightCommandResult(HttpStatusCode code, IEnumerable<ObjectId> lights = null) : base(code)
		{
			Lights = lights;
		}
		public SearchLightCommandResult(HttpStatusCode code, IEnumerable<Notification> notifications) : base(code, notifications) { }

		public IEnumerable<ObjectId> Lights { get; private set; }
	}
}