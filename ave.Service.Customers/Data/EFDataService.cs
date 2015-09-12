using System.Data.Services;
using System.Data.Services.Common;
using System.Data.Services.Providers;

namespace ave.Service.Customers.Data
{
	public class EFDataService : EntityFrameworkDataService<DatabaseContext>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="config"></param>
		public static void InitializeService(DataServiceConfiguration config)
		{
			config.SetEntitySetAccessRule("*", EntitySetRights.All);
			config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
			config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;

			config.UseVerboseErrors = true;
		}
	}
}