using APS.MC.Shared.APSShared.Commands;

namespace APS.MC.Domain.APSContext.Commands.Sensors
{
	public class SearchSensorCommand : ICommand
	{
		public string Term { get; private set; }

		public void SetTerm(string term) => Term = term;
	}
}