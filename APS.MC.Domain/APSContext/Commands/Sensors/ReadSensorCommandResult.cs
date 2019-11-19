using System.Collections.Generic;
using System.Net;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Domain.APSContext.Commands.Sensors
{
	public class ReadSensorCommandResult : CommandResult
	{
		public ReadSensorCommandResult() : base() { }
		public ReadSensorCommandResult(HttpStatusCode code, decimal value) : base(code)
		{
			Value = value;
		}
		public ReadSensorCommandResult(HttpStatusCode code, IEnumerable<Notification> notifications) : base(code, notifications) { }

		public decimal Value { get; private set; }
	}
}