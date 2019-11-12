using System.Collections.Generic;
using System.Linq;
using System.Net;
using APS.MC.Domain.APSContext.Commands.Rooms;
using APS.MC.Domain.APSContext.Entities;
using APS.MC.Domain.APSContext.Repositories;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Commands.Defaults;
using APS.MC.Shared.APSShared.Enums;
using APS.MC.Shared.APSShared.Handlers;
using APS.MC.Shared.APSShared.Notifications;
using MongoDB.Bson;

namespace APS.MC.Domain.APSContext.Handlers
{
	public class RoomCommandHandler : Notifiable,
									ICommandHandler<CreateRoomCommand, CreateEntityCommandResult>,
									ICommandHandler<UpdateRoomCommand, CommandResult>,
									ICommandHandler<GetRoomCommand, GetRoomCommandResult>,
									ICommandHandler<SearchRoomCommand, SearchRoomCommandResult>,
									ICommandHandler<DeleteRoomCommand, CommandResult>
	{
		private readonly IRoomRepository _roomRepository;
		private readonly IArduinoCommunicationService _arduinoCommunicationService;

		public RoomCommandHandler(IRoomRepository roomRepository, IArduinoCommunicationService arduinoCommunicationService)
		{
			_roomRepository = roomRepository;
			_arduinoCommunicationService = arduinoCommunicationService;
		}

		public CreateEntityCommandResult Handle(CreateRoomCommand command)
		{
			CreateEntityCommandResult result = new CreateEntityCommandResult();

			Room room = new Room(command.Description);

			if (room.Valid)
			{
				_roomRepository.Create(room);

				if (_roomRepository.Valid)
					result = new CreateEntityCommandResult(HttpStatusCode.OK).Build<Room, CreateEntityCommandResult>(room);
			}

			else
				result = new CreateEntityCommandResult(HttpStatusCode.BadRequest, room.Notifications);

			return result;
		}

		public CommandResult Handle(UpdateRoomCommand command)
		{
			CommandResult result = new CommandResult();

			ObjectId roomId = new ObjectId();

			if (!ObjectId.TryParse(command.RoomId, out roomId))
				AddNotification(nameof(command.RoomId), ENotifications.InvalidFormat);

			if (Valid)
			{
				Room room = _roomRepository.Get(roomId);

				if (room == null && _roomRepository.Valid)
					AddNotification(nameof(command.RoomId), ENotifications.NotFound);

				if (Valid)
				{
					room.Update(command.Description);

					if (room.Valid)
					{
						_roomRepository.Update(room);

						if (_roomRepository.Valid)
							result = new CommandResult(HttpStatusCode.OK);
					}

					else
						result = new CommandResult(HttpStatusCode.BadRequest, room.Notifications);
				}

				else
					result = new CommandResult(HttpStatusCode.BadRequest, Notifications);
			}

			else
				result = new CommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public GetRoomCommandResult Handle(GetRoomCommand command)
		{
			GetRoomCommandResult result = new GetRoomCommandResult();

			ObjectId roomId = new ObjectId();

			if (!ObjectId.TryParse(command.RoomId, out roomId))
				AddNotification(nameof(command.RoomId), ENotifications.InvalidFormat);

			if (Valid)
			{
				Room room = _roomRepository.Get(roomId);

				if (room == null && _roomRepository.Valid)
					AddNotification(nameof(command.RoomId), ENotifications.NotFound);

				if (Valid)
					result = new GetRoomCommandResult(HttpStatusCode.OK).Build<Room, GetRoomCommandResult>(room, command.Fields);

				else
					result = new GetRoomCommandResult(HttpStatusCode.BadRequest, Notifications);
			}

			else
				result = new GetRoomCommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public SearchRoomCommandResult Handle(SearchRoomCommand command)
		{
			SearchRoomCommandResult result = new SearchRoomCommandResult();

			List<ObjectId> rooms = _roomRepository.Search(command.Term).ToList();

			if (rooms.Count > 0)
				result = new SearchRoomCommandResult(HttpStatusCode.OK, rooms);

			else if (_roomRepository.Valid)
				result = new SearchRoomCommandResult(HttpStatusCode.NoContent);

			return result;
		}

		public CommandResult Handle(DeleteRoomCommand command)
		{
			CommandResult result = new CommandResult();

			ObjectId roomId = new ObjectId();

			if (!ObjectId.TryParse(command.RoomId, out roomId))
				AddNotification(nameof(command.RoomId), ENotifications.InvalidFormat);

			if (Valid)
			{
				Room room = _roomRepository.Get(roomId);

				if (room == null && _roomRepository.Valid)
					AddNotification(nameof(command.RoomId), ENotifications.NotFound);

				if (Valid)
				{
					_roomRepository.Delete(room);

					if (_roomRepository.Valid)
						result = new CommandResult(HttpStatusCode.OK);
				}

				else
					result = new CommandResult(HttpStatusCode.BadRequest, Notifications);
			}

			else
				result = new CommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}
	}
}