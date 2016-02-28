using Q2.Library.Common.Configuration;
using Q2.Library.EventStore.Configuration;
using Q2.Oao.Library.Logging.Configuration;

namespace Q2.Cqrs.Server.Api.Configuration
{
	public class AppSettings
	{
		public LogSettings LogSettings { get; set; }

		public string CorsOrigins { get; set; } 

		public ServiceSettings ServiceSettings { get; set; }

		public EventStoreSettings EventStoreSettings { get; set; }

		public string MongoDatabase { get; set; }

	}
}