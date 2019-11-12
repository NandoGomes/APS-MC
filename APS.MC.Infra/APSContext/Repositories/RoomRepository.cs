using System;
using System.Collections.Generic;
using System.Linq;
using APS.MC.Domain.APSContext.Entities;
using APS.MC.Domain.APSContext.Repositories;
using APS.MC.Infra.APSContext.DataContext;
using APS.MC.Shared.APSShared.Enums;
using APS.MC.Shared.APSShared.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace APS.MC.Infra.APSContext.Repositories
{
	public class RoomRepository : Repository<Room>, IRoomRepository
	{
		public RoomRepository(APSDataContext dataContext, ILoggingService loggingService) : base("rooms", dataContext, loggingService) { }

		public IEnumerable<ObjectId> Search(string term)
		{
			IEnumerable<ObjectId> result = new List<ObjectId>();

			try
			{
				result = _collection.Find<Room>(entity => entity.Description.ToLower().Contains(term)).ToEnumerable().Select(entity => entity.Id);
			}
			catch (Exception e)
			{
				_loggingService.Log(ELogType.Neutral, ELogLevel.Error, e, term);
				AddNotification("Error", e.Message);
			}

			return result;
		}
	}
}