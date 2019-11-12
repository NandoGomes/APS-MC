using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using APS.MC.Domain.APSContext.Commands.Buzzers;
using APS.MC.Domain.APSContext.Entities;
using APS.MC.Domain.APSContext.Repositories;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Queries.Buzzers;
using APS.MC.Domain.APSContext.ValueObjects;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Commands.Defaults;
using APS.MC.Shared.APSShared.Enums;
using APS.MC.Shared.APSShared.Handlers;
using APS.MC.Shared.APSShared.Notifications;
using MongoDB.Bson;

namespace APS.MC.Domain.APSContext.Handlers
{
	public class BuzzerCommandHandler : Notifiable,
										ICommandHandler<CreateBuzzerCommand, CreateEntityCommandResult>,
										ICommandHandler<UpdateBuzzerCommand, CommandResult>,
										ICommandHandler<GetBuzzerCommand, GetBuzzerCommandResult>,
										ICommandHandler<SearchBuzzerCommand, SearchBuzzerCommandResult>,
										ICommandHandler<SearchBuzzerByRoomCommand, SearchBuzzerByRoomCommandResult>,
										ICommandHandler<DeleteBuzzerCommand, CommandResult>,
										ICommandHandler<SwitchBuzzerCommand, Task<CommandResult>>
	{
		private readonly IBuzzerRepository _buzzerRepository;
		private readonly IRoomRepository _roomRepository;
		private readonly IArduinoCommunicationService _arduinoCommunicationService;

		public BuzzerCommandHandler(IBuzzerRepository buzzerRepository, IRoomRepository roomRepository, IArduinoCommunicationService arduinoCommunicationService)
		{
			_buzzerRepository = buzzerRepository;
			_roomRepository = roomRepository;
			_arduinoCommunicationService = arduinoCommunicationService;
		}

		public CreateEntityCommandResult Handle(CreateBuzzerCommand command)
		{
			CreateEntityCommandResult result = new CreateEntityCommandResult();

			ObjectId roomId;
			if (!ObjectId.TryParse(command.RoomId, out roomId))
				AddNotification(nameof(roomId), ENotifications.InvalidFormat);

			if (Valid)
			{
				PinPort pinPort = new PinPort(command.PinPort);
				Buzzer buzzer = new Buzzer(command.Description, pinPort, roomId);

				if (buzzer.Valid)
				{
					if (_roomRepository.Get(roomId) == null)
						AddNotification(nameof(command.RoomId), ENotifications.NotFound);

					if (Valid)
					{
						_buzzerRepository.Create(buzzer);

						if (_buzzerRepository.Valid)
							result = new CreateEntityCommandResult(HttpStatusCode.OK).Build<Buzzer, CreateEntityCommandResult>(buzzer);
					}

					else
						result = new CreateEntityCommandResult(HttpStatusCode.BadRequest, Notifications);
				}

				else
					result = new CreateEntityCommandResult(HttpStatusCode.BadRequest, buzzer.Notifications);
			}

			else
				result = new CreateEntityCommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public CommandResult Handle(UpdateBuzzerCommand command)
		{
			CommandResult result = new CommandResult();

			ObjectId buzzerId = new ObjectId();

			if (!ObjectId.TryParse(command.BuzzerId, out buzzerId))
				AddNotification(nameof(command.BuzzerId), ENotifications.InvalidFormat);

			if (Valid)
			{
				Buzzer buzzer = _buzzerRepository.Get(buzzerId);

				if (buzzer == null && _buzzerRepository.Valid)
					AddNotification(nameof(command.BuzzerId), ENotifications.NotFound);

				if (Valid)
				{
					PinPort pinPort = null;

					if (command.PinPort != null)
						pinPort = new PinPort(command.PinPort);

					buzzer.Update(command.Description, pinPort);

					if (buzzer.Valid)
					{
						_buzzerRepository.Update(buzzer);

						if (_buzzerRepository.Valid)
							result = new CommandResult(HttpStatusCode.OK);
					}

					else
						result = new CommandResult(HttpStatusCode.BadRequest, buzzer.Notifications);
				}

				else
					result = new CommandResult(HttpStatusCode.BadRequest, Notifications);
			}

			else
				result = new CommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public GetBuzzerCommandResult Handle(GetBuzzerCommand command)
		{
			GetBuzzerCommandResult result = new GetBuzzerCommandResult();

			ObjectId buzzerId = new ObjectId();
			if (!ObjectId.TryParse(command.BuzzerId, out buzzerId))
				AddNotification(nameof(command.BuzzerId), ENotifications.InvalidFormat);

			if (Valid)
			{
				Buzzer buzzer = _buzzerRepository.Get(buzzerId);

				if (buzzer != null)
					result = new GetBuzzerCommandResult(HttpStatusCode.OK).Build<Buzzer, GetBuzzerCommandResult>(buzzer, command.Fields);

				else if (_buzzerRepository.Valid)
					result = new GetBuzzerCommandResult(HttpStatusCode.NoContent);
			}

			else
				result = new GetBuzzerCommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public SearchBuzzerCommandResult Handle(SearchBuzzerCommand command)
		{
			SearchBuzzerCommandResult result = new SearchBuzzerCommandResult();

			List<ObjectId> buzzers = _buzzerRepository.Search(command.Term).ToList();

			if (buzzers.Count > 0)
				result = new SearchBuzzerCommandResult(HttpStatusCode.OK, buzzers);

			else if (_buzzerRepository.Valid)
				result = new SearchBuzzerCommandResult(HttpStatusCode.NoContent);

			return result;
		}

		public SearchBuzzerByRoomCommandResult Handle(SearchBuzzerByRoomCommand command)
		{
			SearchBuzzerByRoomCommandResult result = new SearchBuzzerByRoomCommandResult();

			ObjectId roomId;
			if (!ObjectId.TryParse(command.RoomId, out roomId))
				AddNotification(nameof(command.RoomId), ENotifications.InvalidFormat);

			if (Valid)
			{
				List<ObjectId> buzzers = _buzzerRepository.SearchByRoom(roomId).ToList();

				if (buzzers.Count > 0)
					result = new SearchBuzzerByRoomCommandResult(HttpStatusCode.OK, buzzers);

				else if (_buzzerRepository.Valid)
					result = new SearchBuzzerByRoomCommandResult(HttpStatusCode.NoContent);
			}

			else
				result = new SearchBuzzerByRoomCommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public CommandResult Handle(DeleteBuzzerCommand command)
		{
			CommandResult result = new CommandResult();

			ObjectId buzzerId = new ObjectId();
			if (!ObjectId.TryParse(command.BuzzerId, out buzzerId))
				AddNotification(nameof(command.BuzzerId), ENotifications.InvalidFormat);

			if (Valid)
			{
				Buzzer buzzer = _buzzerRepository.Get(buzzerId);

				if (buzzer == null && _buzzerRepository.Valid)
					AddNotification(nameof(command.BuzzerId), ENotifications.NotFound);

				if (Valid)
				{
					_buzzerRepository.Delete(buzzer);

					if (_buzzerRepository.Valid)
						result = new CommandResult(HttpStatusCode.OK);
				}

				else
					result = new CommandResult(HttpStatusCode.BadRequest, Notifications);
			}

			else
				result = new CommandResult(HttpStatusCode.BadRequest, Notifications);

			return result;
		}

		public async Task<CommandResult> Handle(SwitchBuzzerCommand command)
		{
			CommandResult result = new CommandResult();

			ObjectId buzzerId = new ObjectId();
			if (!ObjectId.TryParse(command.BuzzerId, out buzzerId))
				AddNotification(nameof(command.BuzzerId), ENotifications.InvalidFormat);

			if (Valid)
			{
				Buzzer buzzer = _buzzerRepository.Get(buzzerId);

				if (buzzer == null && _buzzerRepository.Valid)
					AddNotification(nameof(command.BuzzerId), ENotifications.NotFound);

				if (Valid)
				{
					await _arduinoCommunicationService.Buzzers.Switch(new SwitchBuzzerQuery(buzzer.PinPort, !buzzer.State));

					if (_arduinoCommunicationService.Buzzers.Valid)
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