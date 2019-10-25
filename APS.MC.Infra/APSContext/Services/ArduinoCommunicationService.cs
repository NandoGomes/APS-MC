using System.IO.Ports;
using APS.MC.Domain.APSContext.Services;
using APS.MC.Shared.APSShared;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Infra.CommonContext.Services
{
	public class ArduinoCommunicationService : Notifiable, IArduinoCommunicationService
	{
		private readonly SerialPort _communicationPort;

		public ArduinoCommunicationService() => _communicationPort = new SerialPort(Settings.SerialPortName, Settings.SerialPortRate);

		private static void _sendMessage()
		{

		}

		void dispose() => _communicationPort.Close();
	}
}