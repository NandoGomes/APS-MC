using System.Collections.Generic;
using System.Net;
using APS.MC.Shared.APSShared.Notifications;
using MongoDB.Bson;

namespace APS.MC.Shared.APSShared.Commands.Defaults
{
	public class CreateEntityCommandResult : CommandResult
	{
		public CreateEntityCommandResult() : base() { }
		public CreateEntityCommandResult(HttpStatusCode code) : base(code) { }
		public CreateEntityCommandResult(HttpStatusCode code, IEnumerable<Notification> notifications) : base(code, notifications) { }

		public ObjectId Id { get; set; }
	}
}