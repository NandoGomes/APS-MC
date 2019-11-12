using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using APS.MC.Domain.APSContext.Commands.Sensors;
using APS.MC.Domain.APSContext.Entities;
using APS.MC.Domain.APSContext.Repositories;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Queries.Sensors;
using APS.MC.Domain.APSContext.ValueObjects;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Commands.Defaults;
using APS.MC.Shared.APSShared.Enums;
using APS.MC.Shared.APSShared.Handlers;
using APS.MC.Shared.APSShared.Notifications;
using MongoDB.Bson;

namespace APS.MC.Domain.APSContext.Handlers
{
	public class SensorCommandHandler : Notifiable,
										ICommandHandler<CreateSensorCommand, CreateEntityCommandResult>,
										ICommandHandler<UpdateSensorCommand, CommandResult>,
										ICommandHandler<GetSensorCommand, GetSensorCommandResult>,
										ICommandHandler<SearchSensorCommand, SearchSensorCommandResult>,
										ICommandHandler<SearchSensorByRoomCommand, SearchSensorByRoomCommandResult>,
										ICommandHandler<DeleteSensorCommand, CommandResult>,
										ICommandHandler<ReadSensorCommand, Task<ReadSensorCommandResult>>
	{
		private readonly ISensorRepository _sensorRepository;
		private readonly IRoomRepository _roomRepository;
		private readonly IArduinoCommunicationService _arduinoCommunicationService;

		public SensorCommandHandler(ISensorRepository sensorRepository, IRoomRepository roomRepository, IArduinoCommunicationService arduinoCommunicationService)
		{
			_sensorRepository = sensorRepository;
			_roomRepository = roomRepository;
			_arduinoCommunicationService = arduinoCommunicationService;
		}

		public CreateEntityCommandResult Handle(CreateSensorCommand command)
		{
			CreateEntityCommandResult result = new CreateEntityCommandResult();

			ObjectId roomId;
			if (!ObjectId.TryParse(command.RoomId, out roomId))
				AddNotification(nameof(command.RoomId), ENotifications.InvalidFormat);

			if (Valid)
			{
				PinPort pinPort = new PinPort(command.PinPort);
				Sensor sensor = new Sensor(command.Description, pinPort, command.Type, roomId);

				if (sensor.Valid)
				{
					if (_roomRepository.Get(roomId) == null)
						AddNotification(nameof(command.RoomId), ENotifications.NotFound);

					if (Valid)
					{
						_sensorRepository.Create(sensor);

						if (_sensorRepository.Valid)
							result = new CreateEntityCommandResult(HttpStatusCode.OK).Build<Sensor, CreateEntityCommandResult>(sensor);
					}

					else
						result = new CreateEntityCommandResult(HttpStatusCode.BadRequest, Notifications);
				}

				else
					result = new CreateEntityCommandResult(HttpStatusCode.BadRequest, sensor.Notifications);
			}

			else
				result = new CreateEntityCommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public CommandResult Handle(UpdateSensorCommand command)
		{
			CommandResult result = new CommandResult();

			ObjectId sensorId = new ObjectId();

			if (!ObjectId.TryParse(command.SensorId, out sensorId))
				AddNotification(nameof(command.SensorId), ENotifications.InvalidFormat);

			if (Valid)
			{
				Sensor sensor = _sensorRepository.Get(sensorId);

				if (sensor == null && _sensorRepository.Valid)
					AddNotification(nameof(command.SensorId), ENotifications.NotFound);

				if (Valid)
				{
					PinPort pinPort = null;

					if (command.PinPort != null)
						pinPort = new PinPort(command.PinPort);

					sensor.Update(command.Description, pinPort, command.Type);

					if (sensor.Valid)
					{
						_sensorRepository.Update(sensor);

						if (_sensorRepository.Valid)
							result = new CommandResult(HttpStatusCode.OK);
					}

					else
						result = new CommandResult(HttpStatusCode.BadRequest, sensor.Notifications);
				}

				else
					result = new CommandResult(HttpStatusCode.BadRequest, Notifications);
			}

			else
				result = new CommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public GetSensorCommandResult Handle(GetSensorCommand command)
		{
			GetSensorCommandResult result = new GetSensorCommandResult();

			ObjectId sensorId = new ObjectId();
			if (!ObjectId.TryParse(command.SensorId, out sensorId))
				AddNotification(nameof(command.SensorId), ENotifications.InvalidFormat);

			if (Valid)
			{
				Sensor sensor = _sensorRepository.Get(sensorId);

				if (sensor != null)
					result = new GetSensorCommandResult(HttpStatusCode.OK).Build<Sensor, GetSensorCommandResult>(sensor, command.Fields);

				else if (_sensorRepository.Valid)
					result = new GetSensorCommandResult(HttpStatusCode.NoContent);
			}

			else
				result = new GetSensorCommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public SearchSensorCommandResult Handle(SearchSensorCommand command)
		{
			SearchSensorCommandResult result = new SearchSensorCommandResult();

			List<ObjectId> sensors = _sensorRepository.Search(command.Term).ToList();

			if (sensors.Count > 0)
				result = new SearchSensorCommandResult(HttpStatusCode.OK, sensors);

			else if (_sensorRepository.Valid)
				result = new SearchSensorCommandResult(HttpStatusCode.NoContent);

			return result;
		}

		public SearchSensorByRoomCommandResult Handle(SearchSensorByRoomCommand command)
		{
			SearchSensorByRoomCommandResult result = new SearchSensorByRoomCommandResult();

			ObjectId roomId;
			if (!ObjectId.TryParse(command.RoomId, out roomId))
				AddNotification(nameof(command.RoomId), ENotifications.InvalidFormat);

			if (Valid)
			{
				List<ObjectId> sensors = _sensorRepository.SearchByRoom(roomId).ToList();

				if (sensors.Count > 0)
					result = new SearchSensorByRoomCommandResult(HttpStatusCode.OK, sensors);

				else if (_sensorRepository.Valid)
					result = new SearchSensorByRoomCommandResult(HttpStatusCode.NoContent);
			}

			else
				result = new SearchSensorByRoomCommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public CommandResult Handle(DeleteSensorCommand command)
		{
			CommandResult result = new CommandResult();

			ObjectId sensorId = new ObjectId();
			if (!ObjectId.TryParse(command.SensorId, out sensorId))
				AddNotification(nameof(command.SensorId), ENotifications.InvalidFormat);

			if (Valid)
			{
				Sensor sensor = _sensorRepository.Get(sensorId);

				if (sensor == null && _sensorRepository.Valid)
					AddNotification(nameof(command.SensorId), ENotifications.NotFound);

				if (Valid)
				{
					_sensorRepository.Delete(sensor);

					if (_sensorRepository.Valid)
						result = new CommandResult(HttpStatusCode.OK);
				}

				else
					result = new CommandResult(HttpStatusCode.BadRequest, Notifications);
			}

			else
				result = new CommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public async Task<ReadSensorCommandResult> Handle(ReadSensorCommand command)
		{
			ReadSensorCommandResult result = new ReadSensorCommandResult();

			ObjectId sensorId = new ObjectId();
			if (!ObjectId.TryParse(command.SensorId, out sensorId))
				AddNotification(nameof(command.SensorId), ENotifications.InvalidFormat);

			if (Valid)
			{
				Sensor sensor = _sensorRepository.Get(sensorId);

				if (sensor == null && _sensorRepository.Valid)
					AddNotification(nameof(command.SensorId), ENotifications.NotFound);

				if (Valid)
				{
					string value = await _arduinoCommunicationService.Sensors.GetValue(new GetSensorValueQuery(sensor.PinPort, sensor.Type));

					if (_arduinoCommunicationService.Sensors.Valid)
						result = new ReadSensorCommandResult(HttpStatusCode.OK, value);
				}

				else
					result = new ReadSensorCommandResult(HttpStatusCode.BadRequest, Notifications);
			}

			else
				result = new ReadSensorCommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}
	}
}