using System.Collections.Generic;
using System.Net;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Notifications;
using MongoDB.Bson;

namespace APS.MC.Domain.APSContext.Commands.Buzzers
{
	public class SearchBuzzerCommandResult : CommandResult
	{
		public SearchBuzzerCommandResult() : base() { }
		public SearchBuzzerCommandResult(HttpStatusCode code, IEnumerable<ObjectId> buzzers = null) : base(code)
		{
			Buzzers = buzzers;
		}
		public SearchBuzzerCommandResult(HttpStatusCode code, IEnumerable<Notification> notifications) : base(code, notifications) { }

		public IEnumerable<ObjectId> Buzzers { get; private set; }
	}
}