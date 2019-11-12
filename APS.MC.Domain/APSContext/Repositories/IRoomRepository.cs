using System.Collections.Generic;
using APS.MC.Domain.APSContext.Entities;
using APS.MC.Shared.APSShared.Repositories;
using MongoDB.Bson;

namespace APS.MC.Domain.APSContext.Repositories
{
	public interface IRoomRepository : IRepository<Room>
	{
		IEnumerable<ObjectId> Search(string term);
	}
}