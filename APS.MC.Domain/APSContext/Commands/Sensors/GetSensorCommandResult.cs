using System.Collections.Generic;
using System.Net;
using APS.MC.Domain.APSContext.Enums;
using APS.MC.Domain.APSContext.ValueObjects;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Domain.APSContext.Commands.Sensors
{
	public class GetSensorCommandResult : CommandResult
	{
		public GetSensorCommandResult() : base() { }
		public GetSensorCommandResult(HttpStatusCode code) : base(code) { }
		public GetSensorCommandResult(HttpStatusCode code, IEnumerable<Notification> notifications) : base(code, notifications) { }

		public string Description { get; set; }
		public PinPort PinPort { get; set; }
		public ESensorType? Type { get; set; }
	}
}