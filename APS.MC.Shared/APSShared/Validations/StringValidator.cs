using System.Text.RegularExpressions;
using APS.MC.Shared.APSShared.Enums;

namespace APS.MC.Shared.APSShared.Validations
{
	public partial class Validator
	{
		public Validator NotNull(string value, string property, ENotifications notification)
		{
			if (value == null)
				AddNotification(property, notification);

			return this;
		}

		public Validator NotNullOrEmpty(string value, string property, ENotifications notification)
		{
			if (string.IsNullOrEmpty(value))
				AddNotification(property, notification);

			return this;
		}

		public Validator MinLength(string value, int length, string property, ENotifications notification)
		{
			if ((value ?? string.Empty).Length < length)
				AddNotification(property, notification);

			return this;
		}

		public Validator MaxLength(string value, int length, string property, ENotifications notification)
		{
			if ((value ?? string.Empty).Length > length)
				AddNotification(property, notification);

			return this;
		}

		public Validator Matchs(string value, string pattern, string property, ENotifications notifications)
		{
			if (!Regex.IsMatch(value ?? string.Empty, pattern ?? string.Empty))
				AddNotification(property, notifications);

			return this;
		}
	}
}