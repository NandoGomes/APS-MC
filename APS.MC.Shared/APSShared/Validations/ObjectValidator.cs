using APS.MC.Shared.APSShared.Enums;

namespace APS.MC.Shared.APSShared.Validations
{
	public partial class Validator
	{
		public Validator NotNull(object value, string property, ENotifications notification)
		{
			if (value == null)
				AddNotification(property, notification);

			return this;
		}
	}
}