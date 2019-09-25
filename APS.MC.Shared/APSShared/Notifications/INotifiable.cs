using System.Collections.Generic;

namespace APS.MC.Shared.APSShared.Notifications
{
	public interface INotifiable
	{
		bool Valid { get; }
		IList<Notification> Notifications { get; }
	}
}