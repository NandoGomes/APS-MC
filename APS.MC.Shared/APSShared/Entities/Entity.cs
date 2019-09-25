using APS.MC.Shared.APSShared.Notifications;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APS.MC.Shared.APSShared.Entities
{
	public abstract class Entity : Notifiable
	{
		protected Entity()
		{
			Id = new ObjectId();
		}

		[BsonId]
		public ObjectId Id { get; protected set; }
	}
}