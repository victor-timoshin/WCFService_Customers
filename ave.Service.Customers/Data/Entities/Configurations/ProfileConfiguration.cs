using System.Data.Entity.ModelConfiguration;

namespace ave.Service.Customers.Data.Entities.Configurations
{
	public class ProfileConfiguration : EntityTypeConfiguration<Profile>
	{
		/// <summary>
		/// Конструктор класса по умолчанию
		/// </summary>
		public ProfileConfiguration()
		{
			ToTable("Profiles"); /* устанавливает имя таблицы */
			HasKey(i => i.profileId); /* устанавливает первичный ключ для данного типа */

			HasRequired(i => i.user).WithRequiredDependent(i => i.profile); /* Зависимая */

			Property(i => i.name).IsOptional();
		}
	}
}