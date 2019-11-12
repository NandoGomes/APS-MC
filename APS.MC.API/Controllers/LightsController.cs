using APS.MC.Domain.APSContext.Commands.Lights;
using APS.MC.Domain.APSContext.Handlers;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Commands.Defaults;
using APS.MC.Shared.APSShared.Services;
using Microsoft.AspNetCore.Mvc;

namespace APS.MC.API.Controllers
{
	public class LightsController : APSMCController
	{
		public LightsController(LightCommandHandler handler, ILoggingService loggingService) : base(handler, loggingService) { }

		/// <summary>
		/// Create Light
		/// </summary>
		///
		/// <param name="command">Command with data to create the Light</param>
		///
		/// <response code="200">Successfully Created</response>
		/// <response code="400">Invalid Light</response>
		/// <response code="500">Internal Server Error</response>
		[HttpPost]
		[Route("V1/Lights/")]
		public CreateEntityCommandResult Create([FromBody] CreateLightCommand command)
		{
			if (command == null)
				command = new CreateLightCommand();

			return Execute<CreateLightCommand, CreateEntityCommandResult>(command);
		}

		/// <summary>
		/// Update Light
		/// </summary>
		///
		/// <param name="lightId">Light's Id</param>
		/// <param name="command">Command with data to update the Light</param>
		///
		/// <response code="200">Successfully Updated</response>
		/// <response code="400">Invalid Light data</response>
		/// <response code="500">Internal Server Error</response>
		[HttpPatch]
		[Route("V1/Lights/{lightId}")]
		public CommandResult Update(string lightId, [FromBody] UpdateLightCommand command)
		{
			if (command == null)
				command = new UpdateLightCommand();

			command.SetLightId(lightId);

			return Execute<UpdateLightCommand, CommandResult>(command);
		}

		/// <summary>
		/// Get Light
		/// </summary>
		///
		/// <param name="lightId">Light's Id to retrieve data</param>
		/// <param name="fields">A string of fields separeted by ',' with fields desired to be return, all available data will return if left empty</param>
		///
		/// <response code="200">Returns the Light</response>
		/// <response code="204">Light not found</response>
		/// <response code="400">Invalid data</response>
		/// <response code="500">Internal Server Error</response>
		[HttpGet]
		[Route("V1/Lights/{lightId}/")]
		public GetLightCommandResult Get(string lightId, [FromQuery] string fields)
		{
			GetLightCommand command = new GetLightCommand();

			command.SetLightId(lightId);
			command.SetFields(fields);

			return Execute<GetLightCommand, GetLightCommandResult>(command);
		}

		/// <summary>
		/// Search All Lights
		/// </summary>
		///
		/// <param name="term">Term to be used on the Search</param>
		///
		/// <response code="200">Returns a list of lights Ids</response>
		/// <response code="204">No Light found</response>
		/// <response code="500">Internal Server Error</response>
		[HttpGet]
		[Route("V1/Lights/")]
		public SearchLightCommandResult Search([FromQuery] string term)
		{
			SearchLightCommand command = new SearchLightCommand();

			command.SetTerm(term ?? string.Empty);

			return Execute<SearchLightCommand, SearchLightCommandResult>(command);
		}

		/// <summary>
		/// Search All Lights by Room
		/// </summary>
		///
		/// <param name="room">The room Id to search</param>
		///
		/// <response code="200">Returns a list of lights Ids</response>
		/// <response code="204">No Light found</response>
		/// <response code="500">Internal Server Error</response>
		[HttpGet]
		[Route("V1/Lights/Rooms/{room}")]
		public SearchLightByRoomCommandResult SearchByRoom(string room)
		{
			SearchLightByRoomCommand command = new SearchLightByRoomCommand();

			command.SetRoomId(room);

			return Execute<SearchLightByRoomCommand, SearchLightByRoomCommandResult>(command);
		}

		/// <summary>
		/// Delete Light
		/// </summary>
		///
		/// <param name="lightId">Light's Id</param>
		///
		/// <response code="200">Successfully Updated</response>
		/// <response code="400">Invalid data</response>
		/// <response code="500">Internal Server Error</response>
		[HttpDelete]
		[Route("V1/Lights/{lightId}")]
		public CommandResult Delete(string lightId)
		{
			DeleteLightCommand command = new DeleteLightCommand();

			command.SetLightId(lightId);

			return Execute<DeleteLightCommand, CommandResult>(command);
		}


		/// <summary>
		/// Switch Light Value
		/// </summary>
		///
		/// <param name="lightId">Light's Id to read value</param>
		///
		/// <response code="200">Successfully Switch</response>
		/// <response code="204">Light not found</response>
		/// <response code="400">Invalid data</response>
		/// <response code="500">Internal Server Error</response>
		[HttpPatch]
		[Route("V1/Lights/{lightId}/Switch/")]
		public CommandResult Switch(string lightId)
		{
			SwitchLightCommand command = new SwitchLightCommand();

			command.SetLightId(lightId);

			return Execute<SwitchLightCommand, CommandResult>(command);
		}
	}
}