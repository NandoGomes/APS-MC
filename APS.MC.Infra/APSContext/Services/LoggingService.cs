using System;
using System.Linq;
using System.Threading;
using NLog;
using APS.MC.Shared.APSShared;
using APS.MC.Shared.APSShared.Enums;
using APS.MC.Shared.APSShared.Services;

namespace APS.MC.Infra.APSContext.Services
{
	public class LoggingService : ILoggingService
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		public void Log(string user, Guid requestId, ELogType logType, ELogLevel logLevel, string method, object data) => Save(new
		{
			Id = requestId.ToString(),
			User = user,
			Type = logType,
			DateTime = GetCurrentDateTime(),
			Method = method,
			Data = data
		}, logLevel);

		public void Log(string user, Guid requestId, ELogType logType, ELogLevel logLevel, Exception exception, object data) => Save(new
		{
			Id = requestId.ToString(),
			User = user,
			Type = logType,
			DateTime = GetCurrentDateTime(),
			Exception = new
			{
				Name = exception.TargetSite.Name,
				Type = exception.TargetSite.GetType().FullName,
				Parameters = exception.TargetSite.GetParameters().Select(parameter => parameter.Name),
				DeclaringType = exception.TargetSite.DeclaringType
			},
			StackTrace = exception.StackTrace,
			Message = exception.Message,
			Data = data
		}, logLevel);

		public void Log(ELogType logType, ELogLevel logLevel, string method, object data) => this.Log("--", new Guid(), logType, logLevel, method, data);

		public void Log(ELogType logType, ELogLevel logLevel, Exception exception, object data) => this.Log("--", new Guid(), logType, logLevel, exception, data);

		private string GetCurrentDateTime() => DateTime.Now.ToString(@"MM/dd/yyyy HH:mm:ss.fff");

		private void Save(object data, ELogLevel level)
		{
			string logData = Newtonsoft.Json.JsonConvert.SerializeObject(data);

			new Thread(() =>
			{
				switch (level)
				{
					case ELogLevel.Info:
						logger.Info(logData);

						break;

					case ELogLevel.Debug:
						if (Settings.DetailedLog)
							logger.Debug(logData);

						break;

					case ELogLevel.Warn:
						logger.Warn(logData);

						break;

					case ELogLevel.Error:
						logger.Error(logData);

						break;

					case ELogLevel.Fatal:
						logger.Fatal(logData);

						break;
				}
			}).Start();
		}
	}
}