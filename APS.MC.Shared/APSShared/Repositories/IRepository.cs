using System.Collections.Generic;
using MongoDB.Bson;
using APS.MC.Shared.APSShared.Entities;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Shared.APSShared.Repositories
{
	public interface IRepository<T> : INotifiable where T : Entity
	{
		bool Exists(ObjectId id);
		T Get(ObjectId id);
		IEnumerable<ObjectId> GetAll();
		T Create(T entity);
		void Update(T entity);
		void Delete(T entity);
	}
}