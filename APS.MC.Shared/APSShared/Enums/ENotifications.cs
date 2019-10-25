using System.ComponentModel;

namespace APS.MC.Shared.APSShared.Enums
{
	public enum ENotifications
	{
		[Description("Can't be empty")] Null,
		[Description("Is too long")] TooLong,
		[Description("Not a valid format")] InvalidFormat,
		[Description("Not Found")] NotFound
	}
}