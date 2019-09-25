using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using APS.MC.Infra.CommonContext.DataContext;
using APS.MC.Shared.APSShared.Entities;
using APS.MC.Shared.APSShared.Enums;
using APS.MC.Shared.APSShared.Repositories;
using APS.MC.Shared.APSShared.Services;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Infra.CommonContext.Repositories
{
	public class Repository<T> : Notifiable, IRepository<T> where T : Entity
	{
		protected readonly IMongoCollection<T> _collection;
		protected readonly ILoggingService _loggingService;

		public Repository(string collectionName, APSDataContext dataContext, ILoggingService loggingService)
		{
			_collection = dataContext.Database.GetCollection<T>(collectionName);
			_loggingService = loggingService;
		}

		public T Create(T entity)
		{
			try
			{
				_collection.InsertOne(entity);
			}
			catch (Exception e)
			{
				_loggingService.Log(ELogType.Neutral, ELogLevel.Error, e, entity);
				AddNotification("Error", e.Message);
			}

			return entity;
		}

		public void Delete(T entity)
		{
			try
			{
				_collection.DeleteOne(collectionEntity => collectionEntity.Id == entity.Id);
			}
			catch (Exception e)
			{
				_loggingService.Log(ELogType.Neutral, ELogLevel.Error, e, entity);
				AddNotification("Error", e.Message);
			}
		}

		public bool Exists(ObjectId id)
		{
			bool result = false;

			try
			{
				result = _collection.Find<T>(entity => entity.Id == id).Any();
			}
			catch (Exception e)
			{
				_loggingService.Log(ELogType.Neutral, ELogLevel.Error, e, id);
				AddNotification("Error", e.Message);
			}

			return result;
		}

		public T Get(ObjectId id)
		{
			T result = null;

			try
			{
				result = _collection.Find<T>(entity => entity.Id == id).FirstOrDefault();
			}
			catch (Exception e)
			{
				_loggingService.Log(ELogType.Neutral, ELogLevel.Error, e, id);
				AddNotification("Error", e.Message);
			}

			return result;
		}

		public IEnumerable<ObjectId> GetAll()
		{
			IEnumerable<ObjectId> entities = new List<ObjectId>();

			try
			{
				entities = _collection.Find(entity => true).ToEnumerable().Select(entity => entity.Id);
			}
			catch (Exception e)
			{
				_loggingService.Log(ELogType.Neutral, ELogLevel.Error, e, new { });
				AddNotification("Error", e.Message);
			}

			return entities;
		}

		public void Update(T entity)
		{
			try
			{
				_collection.ReplaceOne(collectionEntity => collectionEntity.Id == entity.Id, entity);
			}
			catch (Exception e)
			{
				_loggingService.Log(ELogType.Neutral, ELogLevel.Error, e, entity);
				AddNotification("Error", e.Message);
			}
		}
	}
}