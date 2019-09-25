using APS.MC.Shared.APSShared.Commands;
using APS.MC.Shared.APSShared.Notifications;

namespace APS.MC.Shared.APSShared.Handlers
{
	public interface ICommandHandler<T, E> : INotifiable where T : ICommand
	{
		E Handle(T command);
	}
}