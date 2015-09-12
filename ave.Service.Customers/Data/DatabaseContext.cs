using ave.Service.Customers.Data.Entities;
using ave.Service.Customers.Data.Entities.Configurations;
using System.Data.Entity;

namespace ave.Service.Customers.Data
{
	public class DatabaseContext : DbContext
	{
		public DbSet<Role> tableRoles { get; set; }
		public DbSet<User> tableUsers { get; set; }
		public DbSet<UserProvider> UserProviders { get; set; }
		public DbSet<Account> tableAccounts { get; set; }
		public DbSet<Profile> tableProfiles { get; set; }

		/// <summary>
		/// Конструктор класса по умолчанию
		/// </summary>
		public DatabaseContext() :
			base(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ave.Service.Customers.ConnectionString"].ConnectionString) /* Строка подключения к базе данных */
		{
	#if DEBUG
			Database.SetInitializer(new DatabaseInitializer());
	#else
			Database.SetInitializer<DatabaseContext>(null);
	#endif

			this.Configuration.ProxyCreationEnabled = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add(new RoleConfiguration());
			modelBuilder.Configurations.Add(new UserConfiguration());
			modelBuilder.Configurations.Add(new UserProviderConfiguration());
			modelBuilder.Configurations.Add(new AccountConfiguration());
			modelBuilder.Configurations.Add(new ProfileConfiguration());

			base.OnModelCreating(modelBuilder);
		}
	}
}