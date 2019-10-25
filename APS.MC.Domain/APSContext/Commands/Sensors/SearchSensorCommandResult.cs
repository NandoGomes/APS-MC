using System.Collections.Generic;
using System.Net;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Notifications;
using MongoDB.Bson;

namespace APS.MC.Domain.APSContext.Commands.Sensors
{
	public class SearchSensorCommandResult : CommandResult
	{
		public SearchSensorCommandResult() : base() { }
		public SearchSensorCommandResult(HttpStatusCode code, IEnumerable<ObjectId> sensors = null) : base(code)
		{
			Sensors = sensors;
		}
		public SearchSensorCommandResult(HttpStatusCode code, IEnumerable<Notification> notifications) : base(code, notifications) { }

		public IEnumerable<ObjectId> Sensors { get; private set; }
	}
}