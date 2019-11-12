using System.Collections.Generic;
using System.Net;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Notifications;
using MongoDB.Bson;

namespace APS.MC.Domain.APSContext.Commands.Sensors
{
	public class SearchSensorByRoomCommandResult : CommandResult
	{
		public SearchSensorByRoomCommandResult() : base() { }
		public SearchSensorByRoomCommandResult(HttpStatusCode code, IEnumerable<ObjectId> sensors = null) : base(code)
		{
			Sensors = sensors;
		}
		public SearchSensorByRoomCommandResult(HttpStatusCode code, IEnumerable<Notification> notifications) : base(code, notifications) { }

		public IEnumerable<ObjectId> Sensors { get; private set; }
	}
}