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
	public class SensorRepository : Repository<Sensor>, ISensorRepository
	{
		public SensorRepository(APSDataContext dataContext, ILoggingService loggingService) : base("sensors", dataContext, loggingService) { }

		public IEnumerable<ObjectId> Search(string term)
		{
			IEnumerable<ObjectId> result = new List<ObjectId>();

			try
			{
				result = _collection.Find<Sensor>(entity => entity.Description.ToLower().Contains(term) || entity.PinPort.Value.ToLower().Contains(term)).ToEnumerable().Select(entity => entity.Id);
			}
			catch (Exception e)
			{
				_loggingService.Log(ELogType.Neutral, ELogLevel.Error, e, term);
				AddNotification("Error", e.Message);
			}

			return result;
		}

		public IEnumerable<ObjectId> SearchByRoom(ObjectId roomId)
		{
			IEnumerable<ObjectId> result = new List<ObjectId>();

			try
			{
				result = _collection.Find<Sensor>(entity => entity.RoomId == roomId).ToEnumerable().Select(entity => entity.Id);
			}
			catch (Exception e)
			{
				_loggingService.Log(ELogType.Neutral, ELogLevel.Error, e, roomId);
				AddNotification("Error", e.Message);
			}

			return result;
		}
	}
}