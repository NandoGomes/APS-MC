using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Shared.APSShared.Validations
{
	public interface IValidatable : INotifiable
	{
		void Validate();
	}
}