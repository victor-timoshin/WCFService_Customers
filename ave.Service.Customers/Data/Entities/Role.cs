using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;

namespace ave.Service.Customers.Data.Entities
{
	[DataServiceKey("roleId")]
	public class Role
	{
		#region Constructors

		/// <summary>
		/// Конструктор класса
		/// </summary>
		public Role()
		{
			users = new HashSet<User>();
		}

		#endregion

		/// <summary>
		/// Идентификатор роли
		/// </summary>
		[Key]
		public Guid roleId { get; set; }

		/// <summary>
		/// Имя роли
		/// </summary>
		public string name { get; set; }

		/// <summary>
		/// Краткое описание
		/// </summary>
		public string description { get; set; }

		#region Relationships with users

		/// <summary>
		/// Коллекция пользователей
		/// </summary>
		public virtual ICollection<User> users { get; set; }

		#endregion
	}
}