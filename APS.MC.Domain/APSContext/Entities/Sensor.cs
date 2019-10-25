using APS.MC.Domain.APSContext.Enums;
using APS.MC.Domain.APSContext.ValueObjects;
using APS.MC.Shared.APSShared.Entities;
using APS.MC.Shared.APSShared.Enums;
using APS.MC.Shared.APSShared.Validations;

namespace APS.MC.Domain.APSContext.Entities
{
	public class Sensor : Entity
	{
		public Sensor(string description, PinPort pinPort, ESensorType type) => Update(description, pinPort, type);

		public string Description { get; private set; }
		public PinPort PinPort { get; private set; }
		public ESensorType Type { get; private set; }

		public void Update(string description = null, PinPort pinPort = null, ESensorType? type = null)
		{
			Description = description ?? Description;
			PinPort = pinPort ?? PinPort;
			Type = type ?? Type;

			Validate();
		}

		private void Validate()
		{
			Validator validator = new Validator();

			validator.NotNullOrEmpty(Description, nameof(Description), ENotifications.Null)
					.NotNull(PinPort, nameof(PinPort), ENotifications.Null);

			AddNotifications(PinPort);

			AddNotifications(validator);
		}
	}
}