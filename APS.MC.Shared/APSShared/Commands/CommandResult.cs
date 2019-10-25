using System.Collections.Generic;
using System.Net;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Shared.APSShared.Commands
{
	public class CommandResult : ICommandResult
	{
		public CommandResult() : this(HttpStatusCode.InternalServerError) { }

		public CommandResult(HttpStatusCode code) : this(code, new List<Notification>()) { }

		public CommandResult(HttpStatusCode code, IEnumerable<Notification> notifications)
		{
			Code = code;
			Notifications = notifications;
		}

		public HttpStatusCode Code { get; private set; }

		public IEnumerable<Notification> Notifications { get; private set; }
	}
}