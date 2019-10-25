namespace APS.MC.Shared.APSShared
{
	public static class Settings
	{
		public static string ConnectionString { get; set; }
		public static string DatabaseName { get; set; }

		public static string SerialPortName { get; set; }
		public static int SerialPortRate { get; set; }

		public static bool DetailedLog { get; set; }
	}
}