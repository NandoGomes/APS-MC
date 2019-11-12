using APS.MC.Domain.APSContext.ValueObjects;
using APS.MC.Shared.APSShared.Entities;
using APS.MC.Shared.APSShared.Enums;
using APS.MC.Shared.APSShared.Validations;
using MongoDB.Bson;

namespace APS.MC.Domain.APSContext.Entities
{
	public class Buzzer : Entity
	{
		protected Buzzer() { }
		public Buzzer(string description, PinPort pinPort, ObjectId roomId)
		{
			RoomId = roomId;

			Update(description, pinPort);
		}

		public string Description { get; private set; }
		public PinPort PinPort { get; private set; }
		public bool State { get; private set; }
		public ObjectId RoomId { get; private set; }

		public void Switch() => State = !State;

		public void Update(string description = null, PinPort pinPort = null)
		{
			Description = description ?? Description;
			PinPort = pinPort ?? PinPort;

			Validate();
		}

		private void Validate()
		{
			Validator validator = new Validator();

			validator.NotNullOrEmpty(Description, nameof(Description), ENotifications.Null)
					.NotNull(PinPort, nameof(PinPort), ENotifications.Null);

			AddNotifications(validator);
		}
	}
}