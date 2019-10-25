using System.Collections.Generic;
using System.Net;
using APS.MC.Domain.APSContext.ValueObjects;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Domain.APSContext.Commands.Lights
{
	public class GetLightCommandResult : CommandResult
	{
		public GetLightCommandResult() : base() { }
		public GetLightCommandResult(HttpStatusCode code) : base(code) { }
		public GetLightCommandResult(HttpStatusCode code, IEnumerable<Notification> notifications) : base(code, notifications) { }

		public string Description { get; set; }
		public PinPort PinPort { get; set; }
		public bool? State { get; set; }
	}
}