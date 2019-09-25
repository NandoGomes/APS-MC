using System;
using System.Reflection;
using System.Threading.Tasks;
using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Enums;
using APS.MC.Shared.APSShared.Services;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Shared.APSShared.Handlers
{
	public class Handler : Notifiable
	{
		private readonly dynamic _handler;
		private readonly ILoggingService _loggingService;

		public Handler(dynamic handler, ILoggingService loggingService)
		{
			_handler = handler;
			_loggingService = loggingService;
		}

		public async Task<E> Handle<T, E>(T command, Guid requestId, string user, string requestHost) where T : ICommand where E : ICommandResult
		{
			E result = (E)Activator.CreateInstance(typeof(E), false);

			_loggingService.Log(user, requestId, ELogType.Neutral, ELogLevel.Debug, MethodBase.GetCurrentMethod().Name, command);

			try
			{
				var handleResult = _handler.Handle(command);

				if (handleResult.GetType().BaseType == typeof(Task<E>) || handleResult.GetType().BaseType == typeof(Task))
					result = await handleResult;

				else
					result = handleResult;
			}
			catch (Exception e)
			{
				_loggingService.Log(user, requestId, ELogType.Neutral, ELogLevel.Debug, e, command);
			}

			_loggingService.Log(user, requestId, ELogType.Neutral, ELogLevel.Debug, MethodBase.GetCurrentMethod().Name, result);

			return result;
		}
	}
}