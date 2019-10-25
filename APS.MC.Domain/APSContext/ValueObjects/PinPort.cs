using APS.MC.Shared.APSShared.Enums;
using APS.MC.Shared.APSShared.Validations;
using APS.MC.Shared.APSShared.ValueObjects;

namespace APS.MC.Domain.APSContext.ValueObjects
{
	public class PinPort : ValueObject
	{
		public PinPort(string value)
		{
			Value = value;

			Validate();
		}

		public string Value { get; private set; }

		private void Validate()
		{
			Validator validator = new Validator();

			validator.NotNullOrEmpty(Value, $"{GetThisName()}.{nameof(Value)}", ENotifications.Null)
					.MaxLength(Value, 2, $"{GetThisName()}.{nameof(Value)}", ENotifications.TooLong)
					.Matchs(Value, @"^A?[\d]{1,2}$", $"{GetThisName()}.{nameof(Value)}", ENotifications.InvalidFormat);

			AddNotifications(validator);
		}
	}
}