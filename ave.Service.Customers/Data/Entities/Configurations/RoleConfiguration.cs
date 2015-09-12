using System.Data.Entity.ModelConfiguration;

namespace ave.Service.Customers.Data.Entities.Configurations
{
	public class RoleConfiguration : EntityTypeConfiguration<Role>
	{
		/// <summary>
		/// Конструктор класса по умолчанию
		/// </summary>
		public RoleConfiguration()
		{
			ToTable("Roles"); /* устанавливает имя таблицы */
			HasKey(i => i.roleId); /* устанавливает первичный ключ для данного типа */
		}
	}
}