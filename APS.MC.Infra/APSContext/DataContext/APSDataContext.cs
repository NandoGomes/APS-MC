using MongoDB.Driver;
using APS.MC.Shared.APSShared;

namespace APS.MC.Infra.APSContext.DataContext
{
	public class APSDataContext
	{
		public APSDataContext() => Database = new MongoClient(Settings.ConnectionString).GetDatabase(Settings.DatabaseName);

		public IMongoDatabase Database { get; private set; }
	}
}