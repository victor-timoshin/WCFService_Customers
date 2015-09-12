using System.Data.Entity.ModelConfiguration;

namespace ave.Service.Customers.Data.Entities.Configurations
{
	public class UserConfiguration : EntityTypeConfiguration<User>
	{
		/// <summary>
		/// Конструктор класса по умолчанию
		/// </summary>
		public UserConfiguration()
		{
			ToTable("Users"); /* устанавливает имя таблицы */
			HasKey(i => i.userId); /* устанавливает первичный ключ для данного типа */

			Property(i => i.userName).HasMaxLength(20);

			HasRequired(i => i.profile).WithRequiredPrincipal(i => i.user); /* Главная */
			HasRequired(i => i.account).WithRequiredPrincipal(i => i.user);

			HasMany(i => i.friends).WithMany().Map(i =>
			{
				i.ToTable("RelationshipFriends");
				i.MapLeftKey("userId");
				i.MapRightKey("friendId");
			});
		}
	}
}