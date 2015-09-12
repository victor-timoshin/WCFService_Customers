using ave.DTO.Customers.Accounts.Types;
using ave.DTO.Customers.Profiles.Types;
using ave.Service.Customers.Data.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace ave.Service.Customers.Data
{
	public class DatabaseInitializer : DropCreateDatabaseAlways<DatabaseContext>
	{
		protected override void Seed(DatabaseContext context)
		{
			#region Roles

			Role admin = new Role();
			admin.roleId = Guid.NewGuid();
			admin.name = "admin";
			admin.description = "Администратор";
			context.tableRoles.Add(admin);

			// право удалять чужие сообщения
			// право редактировать чужие сообщения
			// право удалять страницы пользователей
			// ограничивать пользователей в правах редактирования и просмотра сайта, банить
			context.tableRoles.Add(new Role
			{
				roleId = Guid.NewGuid(),
				name = "moderator",
				description = "Пользователь, имеющий более широкие права по сравнению с обыкновенными пользователями"
			});

			Role guest = new Role();
			guest.roleId = Guid.NewGuid();
			guest.name = "guest";
			guest.description = "Обыкновенный пользователь";
			context.tableRoles.Add(guest);

			#endregion

			context.SaveChanges();
		}
	}
}