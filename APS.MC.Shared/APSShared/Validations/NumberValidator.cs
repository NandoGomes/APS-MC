using APS.MC.Shared.APSShared.Enums;

namespace APS.MC.Shared.APSShared.Validations
{
	public partial class Validator
	{
		public Validator GreaterThan(double value, double comparer, string property, ENotifications notification)
		{
			if (value <= comparer)
				AddNotification(property, notification);

			return this;
		}

		public Validator GreaterOrEqualsThan(double value, double comparer, string property, ENotifications notification)
		{
			if (value < comparer)
				AddNotification(property, notification);

			return this;
		}

		public Validator LowerThan(double value, double comparer, string property, ENotifications notification)
		{
			if (value >= comparer)
				AddNotification(property, notification);

			return this;
		}

		public Validator LowerOrEqualsThan(double value, double comparer, string property, ENotifications notification)
		{
			if (value > comparer)
				AddNotification(property, notification);

			return this;
		}

		public Validator Equals(double value, double comparer, string property, ENotifications notification)
		{
			if (value != comparer)
				AddNotification(property, notification);

			return this;
		}

		public Validator Between(double value, double from, double to, string property, ENotifications notification)
		{
			if (value < from || value > to)
				AddNotification(property, notification);

			return this;
		}
	}
}