using APS.MC.Domain.APSContext.Commands.Rooms;
using APS.MC.Domain.APSContext.Handlers;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Commands.Defaults;
using APS.MC.Shared.APSShared.Services;
using Microsoft.AspNetCore.Mvc;

namespace APS.MC.API.Controllers
{
	public class RoomsController : APSMCController
	{
		public RoomsController(RoomCommandHandler handler, ILoggingService loggingService) : base(handler, loggingService) { }

		/// <summary>
		/// Create Room
		/// </summary>
		///
		/// <param name="command">Command with data to create the Room</param>
		///
		/// <response code="200">Successfully Created</response>
		/// <response code="400">Invalid Room</response>
		/// <response code="500">Internal Server Error</response>
		[HttpPost]
		[Route("V1/Rooms/")]
		public CreateEntityCommandResult Create([FromBody] CreateRoomCommand command)
		{
			if (command == null)
				command = new CreateRoomCommand();

			return Execute<CreateRoomCommand, CreateEntityCommandResult>(command);
		}

		/// <summary>
		/// Update Room
		/// </summary>
		///
		/// <param name="roomId">Room's Id</param>
		/// <param name="command">Command with data to update the Room</param>
		///
		/// <response code="200">Successfully Updated</response>
		/// <response code="400">Invalid Room data</response>
		/// <response code="500">Internal Server Error</response>
		[HttpPatch]
		[Route("V1/Rooms/{roomId}")]
		public CommandResult Update(string roomId, [FromBody] UpdateRoomCommand command)
		{
			if (command == null)
				command = new UpdateRoomCommand();

			command.SetRoomId(roomId);

			return Execute<UpdateRoomCommand, CommandResult>(command);
		}

		/// <summary>
		/// Get Room
		/// </summary>
		///
		/// <param name="roomId">Room's Id to retrieve data</param>
		/// <param name="fields">A string of fields separeted by ',' with fields desired to be return, all available data will return if left empty</param>
		///
		/// <response code="200">Returns the Room</response>
		/// <response code="204">Room not found</response>
		/// <response code="400">Invalid data</response>
		/// <response code="500">Internal Server Error</response>
		[HttpGet]
		[Route("V1/Rooms/{roomId}/")]
		public GetRoomCommandResult Get(string roomId, [FromQuery] string fields)
		{
			GetRoomCommand command = new GetRoomCommand();

			command.SetRoomId(roomId);
			command.SetFields(fields);

			return Execute<GetRoomCommand, GetRoomCommandResult>(command);
		}

		/// <summary>
		/// Search All Rooms
		/// </summary>
		///
		/// <param name="term">Term to be used on the Search</param>
		///
		/// <response code="200">Returns a list of rooms Ids</response>
		/// <response code="204">No Room found</response>
		/// <response code="500">Internal Server Error</response>
		[HttpGet]
		[Route("V1/Rooms/")]
		public SearchRoomCommandResult Search([FromQuery] string term)
		{
			SearchRoomCommand command = new SearchRoomCommand();

			command.SetTerm(term ?? string.Empty);

			return Execute<SearchRoomCommand, SearchRoomCommandResult>(command);
		}

		/// <summary>
		/// Delete Room
		/// </summary>
		///
		/// <param name="roomId">Room's Id</param>
		///
		/// <response code="200">Successfully Updated</response>
		/// <response code="400">Invalid data</response>
		/// <response code="500">Internal Server Error</response>
		[HttpDelete]
		[Route("V1/Rooms/{roomId}")]
		public CommandResult Delete(string roomId)
		{
			DeleteRoomCommand command = new DeleteRoomCommand();

			command.SetRoomId(roomId);

			return Execute<DeleteRoomCommand, CommandResult>(command);
		}

	}
}