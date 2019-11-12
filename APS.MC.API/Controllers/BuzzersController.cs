using APS.MC.Domain.APSContext.Commands.Buzzers;
using APS.MC.Domain.APSContext.Handlers;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Commands.Defaults;
using APS.MC.Shared.APSShared.Services;
using Microsoft.AspNetCore.Mvc;

namespace APS.MC.API.Controllers
{
	public class BuzzersController : APSMCController
	{
		public BuzzersController(BuzzerCommandHandler handler, ILoggingService loggingService) : base(handler, loggingService) { }

		/// <summary>
		/// Create Buzzer
		/// </summary>
		///
		/// <param name="command">Command with data to create the Buzzer</param>
		///
		/// <response code="200">Successfully Created</response>
		/// <response code="400">Invalid Buzzer</response>
		/// <response code="500">Internal Server Error</response>
		[HttpPost]
		[Route("V1/Buzzers/")]
		public CreateEntityCommandResult Create([FromBody] CreateBuzzerCommand command)
		{
			if (command == null)
				command = new CreateBuzzerCommand();

			return Execute<CreateBuzzerCommand, CreateEntityCommandResult>(command);
		}

		/// <summary>
		/// Update Buzzer
		/// </summary>
		///
		/// <param name="buzzerId">Buzzer's Id</param>
		/// <param name="command">Command with data to update the Buzzer</param>
		///
		/// <response code="200">Successfully Updated</response>
		/// <response code="400">Invalid Buzzer data</response>
		/// <response code="500">Internal Server Error</response>
		[HttpPatch]
		[Route("V1/Buzzers/{buzzerId}")]
		public CommandResult Update(string buzzerId, [FromBody] UpdateBuzzerCommand command)
		{
			if (command == null)
				command = new UpdateBuzzerCommand();

			command.SetBuzzerId(buzzerId);

			return Execute<UpdateBuzzerCommand, CommandResult>(command);
		}

		/// <summary>
		/// Get Buzzer
		/// </summary>
		///
		/// <param name="buzzerId">Buzzer's Id to retrieve data</param>
		/// <param name="fields">A string of fields separeted by ',' with fields desired to be return, all available data will return if left empty</param>
		///
		/// <response code="200">Returns the Buzzer</response>
		/// <response code="204">Buzzer not found</response>
		/// <response code="400">Invalid data</response>
		/// <response code="500">Internal Server Error</response>
		[HttpGet]
		[Route("V1/Buzzers/{buzzerId}/")]
		public GetBuzzerCommandResult Get(string buzzerId, [FromQuery] string fields)
		{
			GetBuzzerCommand command = new GetBuzzerCommand();

			command.SetBuzzerId(buzzerId);
			command.SetFields(fields);

			return Execute<GetBuzzerCommand, GetBuzzerCommandResult>(command);
		}

		/// <summary>
		/// Search All Buzzers
		/// </summary>
		///
		/// <param name="term">Term to be used on the Search</param>
		///
		/// <response code="200">Returns a list of buzzers Ids</response>
		/// <response code="204">No Buzzer found</response>
		/// <response code="500">Internal Server Error</response>
		[HttpGet]
		[Route("V1/Buzzers/")]
		public SearchBuzzerCommandResult Search([FromQuery] string term)
		{
			SearchBuzzerCommand command = new SearchBuzzerCommand();

			command.SetTerm(term ?? string.Empty);

			return Execute<SearchBuzzerCommand, SearchBuzzerCommandResult>(command);
		}

		/// <summary>
		/// Search All Buzzers by Room
		/// </summary>
		///
		/// <param name="room">The room Id to search</param>
		///
		/// <response code="200">Returns a list of buzzers Ids</response>
		/// <response code="204">No Buzzer found</response>
		/// <response code="500">Internal Server Error</response>
		[HttpGet]
		[Route("V1/Buzzers/Rooms/{room}")]
		public SearchBuzzerByRoomCommandResult SearchByRoom(string room)
		{
			SearchBuzzerByRoomCommand command = new SearchBuzzerByRoomCommand();

			command.SetRoomId(room);

			return Execute<SearchBuzzerByRoomCommand, SearchBuzzerByRoomCommandResult>(command);
		}

		/// <summary>
		/// Delete Buzzer
		/// </summary>
		///
		/// <param name="buzzerId">Buzzer's Id</param>
		///
		/// <response code="200">Successfully Updated</response>
		/// <response code="400">Invalid data</response>
		/// <response code="500">Internal Server Error</response>
		[HttpDelete]
		[Route("V1/Buzzers/{buzzerId}")]
		public CommandResult Delete(string buzzerId)
		{
			DeleteBuzzerCommand command = new DeleteBuzzerCommand();

			command.SetBuzzerId(buzzerId);

			return Execute<DeleteBuzzerCommand, CommandResult>(command);
		}


		/// <summary>
		/// Switch Buzzer Value
		/// </summary>
		///
		/// <param name="buzzerId">Buzzer's Id to read value</param>
		///
		/// <response code="200">Successfully Switch</response>
		/// <response code="204">Buzzer not found</response>
		/// <response code="400">Invalid data</response>
		/// <response code="500">Internal Server Error</response>
		[HttpPatch]
		[Route("V1/Buzzers/{buzzerId}/Switch/")]
		public CommandResult Switch(string buzzerId)
		{
			SwitchBuzzerCommand command = new SwitchBuzzerCommand();

			command.SetBuzzerId(buzzerId);

			return Execute<SwitchBuzzerCommand, CommandResult>(command);
		}
	}
}