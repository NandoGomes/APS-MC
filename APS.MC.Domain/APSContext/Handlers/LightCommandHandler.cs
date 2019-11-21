using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using APS.MC.Domain.APSContext.Commands.Lights;
using APS.MC.Domain.APSContext.Entities;
using APS.MC.Domain.APSContext.Repositories;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Queries.Lights;
using APS.MC.Domain.APSContext.ValueObjects;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Commands.Defaults;
using APS.MC.Shared.APSShared.Enums;
using APS.MC.Shared.APSShared.Handlers;
using APS.MC.Shared.APSShared.Notifications;
using MongoDB.Bson;

namespace APS.MC.Domain.APSContext.Handlers
{
	public class LightCommandHandler : Notifiable,
									ICommandHandler<CreateLightCommand, CreateEntityCommandResult>,
									ICommandHandler<UpdateLightCommand, CommandResult>,
									ICommandHandler<GetLightCommand, Task<GetLightCommandResult>>,
									ICommandHandler<SearchLightCommand, SearchLightCommandResult>,
									ICommandHandler<SearchLightByRoomCommand, SearchLightByRoomCommandResult>,
									ICommandHandler<DeleteLightCommand, CommandResult>,
									ICommandHandler<SwitchLightCommand, Task<CommandResult>>
	{
		private readonly ILightRepository _lightRepository;
		private readonly IRoomRepository _roomRepository;
		private readonly IArduinoCommunicationService _arduinoCommunicationService;

		public LightCommandHandler(ILightRepository lightRepository, IRoomRepository roomRepository, IArduinoCommunicationService arduinoCommunicationService)
		{
			_lightRepository = lightRepository;
			_roomRepository = roomRepository;
			_arduinoCommunicationService = arduinoCommunicationService;
		}

		public CreateEntityCommandResult Handle(CreateLightCommand command)
		{
			CreateEntityCommandResult result = new CreateEntityCommandResult();

			ObjectId roomId;
			if (!ObjectId.TryParse(command.RoomId, out roomId))
				AddNotification(nameof(roomId), ENotifications.InvalidFormat);

			if (Valid)
			{
				PinPort pinPort = new PinPort(command.PinPort);
				Light light = new Light(command.Description, pinPort, roomId);

				if (light.Valid)
				{
					if (_roomRepository.Get(roomId) == null)
						AddNotification(nameof(command.RoomId), ENotifications.NotFound);

					if (Valid)
					{
						_lightRepository.Create(light);

						if (_lightRepository.Valid)
							result = new CreateEntityCommandResult(HttpStatusCode.OK).Build<Light, CreateEntityCommandResult>(light);
					}

					else
						result = new CreateEntityCommandResult(HttpStatusCode.BadRequest, Notifications);
				}

				else
					result = new CreateEntityCommandResult(HttpStatusCode.BadRequest, light.Notifications);
			}

			else
				result = new CreateEntityCommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public CommandResult Handle(UpdateLightCommand command)
		{
			CommandResult result = new CommandResult();

			ObjectId lightId = new ObjectId();

			if (!ObjectId.TryParse(command.LightId, out lightId))
				AddNotification(nameof(command.LightId), ENotifications.InvalidFormat);

			if (Valid)
			{
				Light light = _lightRepository.Get(lightId);

				if (light == null && _lightRepository.Valid)
					AddNotification(nameof(command.LightId), ENotifications.NotFound);

				if (Valid)
				{
					PinPort pinPort = null;

					if (command.PinPort != null)
						pinPort = new PinPort(command.PinPort);

					light.Update(command.Description, pinPort);

					if (light.Valid)
					{
						_lightRepository.Update(light);

						if (_lightRepository.Valid)
							result = new CommandResult(HttpStatusCode.OK);
					}

					else
						result = new CommandResult(HttpStatusCode.BadRequest, light.Notifications);
				}

				else
					result = new CommandResult(HttpStatusCode.BadRequest, Notifications);
			}

			else
				result = new CommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public async Task<GetLightCommandResult> Handle(GetLightCommand command)
		{
			GetLightCommandResult result = new GetLightCommandResult();

			ObjectId lightId = new ObjectId();
			if (!ObjectId.TryParse(command.LightId, out lightId))
				AddNotification(nameof(command.LightId), ENotifications.InvalidFormat);

			if (Valid)
			{
				Light light = _lightRepository.Get(lightId);

				if (light != null)
				{
					light = await UpdateState(light);

					result = new GetLightCommandResult(HttpStatusCode.OK).Build<Light, GetLightCommandResult>(light, command.Fields);
				}

				else if (_lightRepository.Valid)
					result = new GetLightCommandResult(HttpStatusCode.NoContent);
			}

			else
				result = new GetLightCommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public SearchLightCommandResult Handle(SearchLightCommand command)
		{
			SearchLightCommandResult result = new SearchLightCommandResult();

			List<ObjectId> lights = _lightRepository.Search(command.Term).ToList();

			if (lights.Count > 0)
				result = new SearchLightCommandResult(HttpStatusCode.OK, lights);

			else if (_lightRepository.Valid)
				result = new SearchLightCommandResult(HttpStatusCode.NoContent);

			return result;
		}

		public SearchLightByRoomCommandResult Handle(SearchLightByRoomCommand command)
		{
			SearchLightByRoomCommandResult result = new SearchLightByRoomCommandResult();

			ObjectId roomId;
			if (!ObjectId.TryParse(command.RoomId, out roomId))
				AddNotification(nameof(command.RoomId), ENotifications.InvalidFormat);

			if (Valid)
			{
				List<ObjectId> lights = _lightRepository.SearchByRoom(roomId).ToList();

				if (lights.Count > 0)
					result = new SearchLightByRoomCommandResult(HttpStatusCode.OK, lights);

				else if (_lightRepository.Valid)
					result = new SearchLightByRoomCommandResult(HttpStatusCode.NoContent);
			}

			else
				result = new SearchLightByRoomCommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public CommandResult Handle(DeleteLightCommand command)
		{
			CommandResult result = new CommandResult();

			ObjectId lightId = new ObjectId();
			if (!ObjectId.TryParse(command.LightId, out lightId))
				AddNotification(nameof(command.LightId), ENotifications.InvalidFormat);

			if (Valid)
			{
				Light light = _lightRepository.Get(lightId);

				if (light == null && _lightRepository.Valid)
					AddNotification(nameof(command.LightId), ENotifications.NotFound);

				if (Valid)
				{
					_lightRepository.Delete(light);

					if (_lightRepository.Valid)
						result = new CommandResult(HttpStatusCode.OK);
				}

				else
					result = new CommandResult(HttpStatusCode.BadRequest, Notifications);
			}

			else
				result = new CommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public async Task<CommandResult> Handle(SwitchLightCommand command)
		{
			CommandResult result = new CommandResult();

			ObjectId lightId = new ObjectId();
			if (!ObjectId.TryParse(command.LightId, out lightId))
				AddNotification(nameof(command.LightId), ENotifications.InvalidFormat);

			if (Valid)
			{
				Light light = _lightRepository.Get(lightId);

				if (light == null && _lightRepository.Valid)
					AddNotification(nameof(command.LightId), ENotifications.NotFound);

				if (Valid)
				{
					light = await UpdateState(light);

					light.Switch();

					if (await _arduinoCommunicationService.Lights.Switch(new SwitchLightQuery(light.PinPort, light.State)))
					{
						_lightRepository.Update(light);

						if (_lightRepository.Valid)
							result = new CommandResult(HttpStatusCode.OK);
					}
				}

				else
					result = new CommandResult(HttpStatusCode.BadRequest, Notifications);
			}

			else
				result = new CommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		private async Task<Light> UpdateState(Light light)
		{
			if (light.State != await _arduinoCommunicationService.Lights.Read(light.PinPort))
			{
				light.Switch();
				_lightRepository.Update(light);
			}

			return light;
		}
	}
}