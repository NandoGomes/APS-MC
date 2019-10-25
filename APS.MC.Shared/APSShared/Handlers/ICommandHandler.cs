using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Shared.APSShared.Handlers
{
	public interface ICommandHandler<TCommand, TCommandResult> : INotifiable where TCommand : ICommand
	{
		TCommandResult Handle(TCommand command);
	}
}