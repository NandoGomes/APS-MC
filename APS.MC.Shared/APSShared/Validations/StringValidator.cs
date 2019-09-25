namespace APS.MC.Shared.APSShared.Validations
{
	public partial class Validator
	{
		public Validator NotNull(string value, string property, string message)
		{
			if (value == null)
				AddNotification(property, message);

			return this;
		}

		public Validator NotNullOrEmpty(string value, string property, string message)
		{
			if (string.IsNullOrEmpty(value))
				AddNotification(property, message);

			return this;
		}

		public Validator MinLength(string value, int length, string property, string message)
		{
			if ((value ?? string.Empty).Length < length)
				AddNotification(property, message);

			return this;
		}

		public Validator MaxLength(string value, int length, string property, string message)
		{
			if ((value ?? string.Empty).Length > length)
				AddNotification(property, message);

			return this;
		}
	}
}