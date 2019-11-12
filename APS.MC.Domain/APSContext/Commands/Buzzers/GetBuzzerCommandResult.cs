using System.Collections.Generic;
using System.Net;
using APS.MC.Domain.APSContext.ValueObjects;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Notifications;
using MongoDB.Bson;

namespace APS.MC.Domain.APSContext.Commands.Buzzers
{
	public class GetBuzzerCommandResult : CommandResult
	{
		public GetBuzzerCommandResult() : base() { }
		public GetBuzzerCommandResult(HttpStatusCode code) : base(code) { }
		public GetBuzzerCommandResult(HttpStatusCode code, IEnumerable<Notification> notifications) : base(code, notifications) { }

		public ObjectId? Id { get; set; }
		public string Description { get; set; }
		public PinPort PinPort { get; set; }
		public bool? State { get; set; }
		public string Gambis { get => "Buzzer"; }
	}
}