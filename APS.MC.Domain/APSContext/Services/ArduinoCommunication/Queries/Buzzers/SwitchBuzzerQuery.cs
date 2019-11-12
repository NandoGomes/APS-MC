using APS.MC.Domain.APSContext.ValueObjects;

namespace APS.MC.Domain.APSContext.Services.ArduinoCommunication.Queries.Buzzers
{
	public class SwitchBuzzerQuery
	{
		public SwitchBuzzerQuery(PinPort pinPort, bool state)
		{
			PinPort = pinPort;
			State = state;
		}

		public PinPort PinPort { get; private set; }
		public bool State { get; private set; }
	}
}