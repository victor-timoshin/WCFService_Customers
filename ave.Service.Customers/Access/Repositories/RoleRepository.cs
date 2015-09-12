using ave.Service.Customers.Data;
using ave.Service.Customers.Data.Entities;
using ave.Service.Customers.Access.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ave.Service.Customers.Access.Repositories
{
	public class RoleRepository : IRoleRepository
	{
		private readonly EFServiceRepository_Helpers.IUnitOfWork unitOfWork = null;

		/// <summary>
		/// Конструктор класса по умочанию
		/// </summary>
		/// <param name="context"></param>
		public RoleRepository(DatabaseContext context)
		{
			unitOfWork = new EFServiceRepository_Helpers.UnitOfWork(context);
		}

		/// <summary>
		/// Создает новую роль
		/// </summary>
		/// <param name="roleName">Имя роли</param>
		/// <param name="description">Краткое описание</param>
		public void Create(string roleName, string description)
		{
			Role roleEntity = new Role();
			roleEntity.roleId = Guid.NewGuid();
			roleEntity.name = roleName;
			roleEntity.description = description;

			unitOfWork.RepositoryFor<Role>().Add(roleEntity);
			unitOfWork.Commit();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		/// <param name="roleName">Имя роли</param>
		/// <returns></returns>
		public bool AttachByName(string userName, string roleName)
		{
			var userEntity = unitOfWork.RepositoryFor<User>().Single(i => i.userName == userName);
			if (userEntity == null)
				return false;

			var roleEntity = GetRoleByName(roleName);
			if (roleEntity == null)
				return false;

			if (userEntity.role != roleEntity)
			{
				userEntity.role = roleEntity;

				//unitOfWork.RepositoryFor<User>().Attach(userEntity);
				unitOfWork.RepositoryFor<User>().Update(userEntity);
				unitOfWork.Commit();
			}

			return true;
		}

		/// <summary>
		/// Получает данные таблицы
		/// </summary>
		/// <param name="roleName">Имя роли</param>
		/// <returns></returns>
		public Role GetRoleByName(string roleName)
		{
			return unitOfWork.RepositoryFor<Role>().Single(i => i.name == roleName);
		}

		/// <summary>
		/// Получает имя роли, которая прикреплена к пользователя
		/// </summary>
		/// <param name="username">Псевдоним пользователя</param>
		/// <returns></returns>
		public string GetRoleByUserName(string username)
		{
			var roleEntity = unitOfWork.RepositoryFor<User>().First(i => i.userName == username).role;
			return roleEntity.name;
		}

		/// <summary>
		/// Получает все роли из базы данных
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Role> GetRoles()
		{
			return unitOfWork.RepositoryFor<Role>().GetAll();
		}

		/// <summary>
		/// Получает общее количество всех ролей
		/// </summary>
		/// <returns></returns>
		public int GetTotalCount()
		{
			return unitOfWork.RepositoryFor<Role>().GetAll().Count();
		}
	}
}