using System;
using System.Net.Http;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication.Controllers;
using APS.MC.Infra.CommonContext.Services.ArduinoCommunicationService.Controllers;
using APS.MC.Shared.APSShared;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Infra.CommonContext.Services.ArduinoCommunicationService
{
	public class ArduinoCommunicationService : Notifiable, IArduinoCommunicationService
	{
		public ArduinoCommunicationService()
		{
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri(Settings.ArduinoAddress);

			Sensors = new SensorController(client);
		}

		public ISensorController Sensors { get; private set; }
	}
}