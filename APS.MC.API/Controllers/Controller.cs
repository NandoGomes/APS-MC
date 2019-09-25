using System;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Enums;
using APS.MC.Shared.APSShared.Handlers;
using APS.MC.Shared.APSShared.Services;

namespace APS.MC.API.Controllers
{
	public abstract class Controller : Microsoft.AspNetCore.Mvc.Controller
	{
		private readonly dynamic _handler;
		private readonly ILoggingService _loggingService;

		public Controller(dynamic handler, ILoggingService loggingService)
		{
			_handler = handler;
			_loggingService = loggingService;
		}

		protected E Execute<T, E>(T command) where T : ICommand where E : ICommandResult
		{
			Guid requestId = Guid.NewGuid();

			_loggingService.Log(HttpContext.User.Identity.Name, requestId, ELogType.Input, ELogLevel.Info, $"{HttpContext.Request.Method} -> {HttpContext.Request.Path}", command);

			E result = new Handler(_handler, _loggingService).Handle<T, E>(command, requestId, HttpContext.User.Identity.Name, HttpContext.Connection.RemoteIpAddress.ToString()).Result;

			_loggingService.Log(HttpContext.User.Identity.Name, requestId, ELogType.Output, ELogLevel.Info, $"{HttpContext.Request.Method} -> {HttpContext.Request.Path}", result);

			HttpContext.Response.StatusCode = (int)result.Code;

			return result;
		}
	}
}