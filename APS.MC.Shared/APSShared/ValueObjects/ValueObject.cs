using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Shared.APSShared.ValueObjects
{
	public abstract class ValueObject : Notifiable
	{
		protected string GetThisName() => this.GetType().Name;
	}
}