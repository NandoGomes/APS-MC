namespace APS.MC.Domain.APSContext.Services.ArduinoCommunication.Responses
{
	public class ARESTDefaultResponse
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Hardware { get; set; }
		public bool Connected { get; set; }
		public int Return_Fields { get; set; }
	}
}