using APS.MC.Domain.APSContext.ValueObjects;
using APS.MC.Shared.APSShared.Entities;
using APS.MC.Shared.APSShared.Enums;
using APS.MC.Shared.APSShared.Validations;

namespace APS.MC.Domain.APSContext.Entities
{
	public class Light : Entity
	{
		protected Light() { }

		public Light(string description, PinPort pinPort) => Update(description, pinPort);

		public string Description { get; private set; }
		public PinPort PinPort { get; private set; }
		public bool State { get; private set; }

		public void Switch() => State = !State;

		public void Update(string description = "", PinPort pinPort = null)
		{
			Description = description;
			PinPort = pinPort;

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