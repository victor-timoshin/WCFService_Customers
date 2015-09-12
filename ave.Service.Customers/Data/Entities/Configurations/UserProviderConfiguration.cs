using System.Data.Entity.ModelConfiguration;

namespace ave.Service.Customers.Data.Entities.Configurations
{
	public class UserProviderConfiguration : EntityTypeConfiguration<UserProvider>
	{
		/// <summary>
		/// Конструктор класса по умолчанию
		/// </summary>
		public UserProviderConfiguration()
		{
			ToTable("UserProviders"); /* устанавливает имя таблицы */
			HasKey(i => i.providerId); /* устанавливает первичный ключ для данного типа */
		}
	}
}