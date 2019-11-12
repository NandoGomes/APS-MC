using System;
using APS.MC.Shared.APSShared.Entities;
using APS.MC.Shared.APSShared.Enums;
using APS.MC.Shared.APSShared.Validations;

namespace APS.MC.Domain.APSContext.Entities
{
	public class Room : Entity
	{
		public Room(string description) => Update(description);

		public string Description { get; private set; }

		public void Update(string description)
		{
			Description = description;

			Validate();
		}

		private void Validate()
		{
			Validator validator = new Validator();

			validator.NotNullOrEmpty(Description, nameof(Description), ENotifications.Null)
					.MaxLength(Description, 250, nameof(Description), ENotifications.TooLong);

			AddNotifications(validator);
		}
	}
}