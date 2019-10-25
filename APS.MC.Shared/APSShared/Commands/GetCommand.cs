namespace APS.MC.Shared.APSShared.Commands
{
	public abstract class GetCommand : ICommand
	{
		public string Fields { get; private set; }

		public void SetFields(string fields) => Fields = fields;
	}
}