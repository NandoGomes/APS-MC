using APS.MC.Domain.APSContext.Commands.Sensors;
using APS.MC.Domain.APSContext.Handlers;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Commands.Defaults;
using APS.MC.Shared.APSShared.Services;
using Microsoft.AspNetCore.Mvc;

namespace APS.MC.API.Controllers
{
	public class SensorsController : APSMCController
	{
		public SensorsController(SensorCommandHandler handler, ILoggingService loggingService) : base(handler, loggingService) { }

		/// <summary>
		/// Create Sensor
		/// </summary>
		///
		/// <param name="command">Command with data to create the Sensor</param>
		///
		/// <response code="200">Successfully Created</response>
		/// <response code="400">Invalid Sensor</response>
		/// <response code="500">Internal Server Error</response>
		[HttpPost]
		[Route("V1/Sensors/")]
		public CreateEntityCommandResult Create([FromBody] CreateSensorCommand command)
		{
			if (command == null)
				command = new CreateSensorCommand();

			return Execute<CreateSensorCommand, CreateEntityCommandResult>(command);
		}

		/// <summary>
		/// Update Sensor
		/// </summary>
		///
		/// <param name="sensorId">Sensor's Id</param>
		/// <param name="command">Command with data to update the Sensor</param>
		///
		/// <response code="200">Successfully Updated</response>
		/// <response code="400">Invalid Sensor data</response>
		/// <response code="500">Internal Server Error</response>
		[HttpPatch]
		[Route("V1/Sensors/{sensorId}")]
		public CommandResult Update(string sensorId, [FromBody] UpdateSensorCommand command)
		{
			if (command == null)
				command = new UpdateSensorCommand();

			command.SetSensorId(sensorId);

			return Execute<UpdateSensorCommand, CommandResult>(command);
		}

		/// <summary>
		/// Get Sensor
		/// </summary>
		///
		/// <param name="sensorId">Sensor's Id to retrieve data</param>
		/// <param name="fields">A string of fields separeted by ',' with fields desired to be return, all available data will return if left empty</param>
		///
		/// <response code="200">Returns the Sensor</response>
		/// <response code="204">Sensor not found</response>
		/// <response code="400">Invalid data</response>
		/// <response code="500">Internal Server Error</response>
		[HttpGet]
		[Route("V1/Sensors/{sensorId}/")]
		public GetSensorCommandResult Get(string sensorId, [FromQuery] string fields)
		{
			GetSensorCommand command = new GetSensorCommand();

			command.SetSensorId(sensorId);
			command.SetFields(fields);

			return Execute<GetSensorCommand, GetSensorCommandResult>(command);
		}

		/// <summary>
		/// Search All Sensors
		/// </summary>
		///
		/// <param name="term">Term to be used on the Search</param>
		///
		/// <response code="200">Returns a list of sensors Ids</response>
		/// <response code="204">No Sensor found</response>
		/// <response code="500">Internal Server Error</response>
		[HttpGet]
		[Route("V1/Sensors/")]
		public SearchSensorCommandResult Search([FromQuery] string term)
		{
			SearchSensorCommand command = new SearchSensorCommand();

			command.SetTerm(term ?? string.Empty);

			return Execute<SearchSensorCommand, SearchSensorCommandResult>(command);
		}

		/// <summary>
		/// Search All Sensors by Room
		/// </summary>
		///
		/// <param name="room">The room Id to search</param>
		///
		/// <response code="200">Returns a list of sensors Ids</response>
		/// <response code="204">No Sensor found</response>
		/// <response code="500">Internal Server Error</response>
		[HttpGet]
		[Route("V1/Sensors/Rooms/{room}")]
		public SearchSensorByRoomCommandResult SearchByRoom(string room)
		{
			SearchSensorByRoomCommand command = new SearchSensorByRoomCommand();

			command.SetRoomId(room);

			return Execute<SearchSensorByRoomCommand, SearchSensorByRoomCommandResult>(command);
		}

		/// <summary>
		/// Delete Sensor
		/// </summary>
		///
		/// <param name="sensorId">Sensor's Id</param>
		///
		/// <response code="200">Successfully Updated</response>
		/// <response code="400">Invalid data</response>
		/// <response code="500">Internal Server Error</response>
		[HttpDelete]
		[Route("V1/Sensors/{sensorId}")]
		public CommandResult Delete(string sensorId)
		{
			DeleteSensorCommand command = new DeleteSensorCommand();

			command.SetSensorId(sensorId);

			return Execute<DeleteSensorCommand, CommandResult>(command);
		}

		/// <summary>
		/// Read Sensor Value
		/// </summary>
		///
		/// <param name="sensorId">Sensor's Id to read value</param>
		///
		/// <response code="200">Returns the Sensor's current value</response>
		/// <response code="204">Sensor not found</response>
		/// <response code="400">Invalid data</response>
		/// <response code="500">Internal Server Error</response>
		[HttpGet]
		[Route("V1/Sensors/{sensorId}/Read/")]
		public ReadSensorCommandResult Read(string sensorId)
		{
			ReadSensorCommand command = new ReadSensorCommand();

			command.SetSensorId(sensorId);

			return Execute<ReadSensorCommand, ReadSensorCommandResult>(command);
		}
	}
}