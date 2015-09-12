using System.Data.Entity.ModelConfiguration;

namespace ave.Service.Customers.Data.Entities.Configurations
{
	public class AccountConfiguration : EntityTypeConfiguration<Account>
	{
		/// <summary>
		/// Конструктор класса по умолчанию
		/// </summary>
		public AccountConfiguration()
		{
			ToTable("Accounts"); /* устанавливает имя таблицы */
			HasKey(i => i.accountId); /* устанавливает первичный ключ для данного типа */

			HasRequired(i => i.user).WithRequiredDependent();
		}
	}
}