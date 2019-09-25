using System.Collections.Generic;
using System.Net;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Shared.APSShared.Commands
{
	public interface ICommandResult
	{
		HttpStatusCode Code { get; }
		IReadOnlyCollection<Notification> Notifications { get; }
	}
}