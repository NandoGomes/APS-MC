using System;
using APS.MC.Shared.APSShared.Enums;

namespace APS.MC.Shared.APSShared.Services
{
	public interface ILoggingService
	{
		void Log(string user, Guid requestId, ELogType logType, ELogLevel logLevel, string method, object data);
		void Log(string user, Guid requestId, ELogType logType, ELogLevel logLevel, Exception exception, object data);
		void Log(ELogType logType, ELogLevel logLevel, string method, object data);
		void Log(ELogType logType, ELogLevel logLevel, Exception exception, object data);
	}
}