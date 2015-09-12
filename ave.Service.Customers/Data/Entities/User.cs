using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;

namespace ave.Service.Customers.Data.Entities
{
	[DataServiceKey("userId")]
	public class User
	{
		#region Constructors

		/// <summary>
		/// Конструктор класса
		/// </summary>
		public User()
		{
			friends = new HashSet<User>();
			referrals = new HashSet<User>();
		}

		#endregion

		/// <summary>
		/// Уникальный идентификатор
		/// </summary>
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Guid userId { get; set; }

		/// <summary>
		/// Псевдоним пользователя
		/// </summary>
		public string userName { get; set; }

		/// <summary>
		/// Адрес электронной почты
		/// </summary>
		public string email { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string emailKey { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string passwordSalt { get; set; }

		/// <summary>
		/// Пароль
		/// </summary>
		public string password { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool isLockedOut { get; set; }

		/// <summary>
		/// Дата создание
		/// </summary>
		public DateTime dateCreated { get; set; }

		/// <summary>
		/// Дата последнего входа в систему
		/// </summary>
		public DateTime lastDateLogin { get; set; }

		/// <summary>
		/// Дата последней модификации профиля
		/// </summary>
		public DateTime lastDateModified { get; set; }

		/// <summary>
		/// Дата последней блокировки аккаунта
		/// </summary>
		public DateTime lastDateLockout { get; set; }

		/// <summary>
		/// Дата последней смены пароля
		/// </summary>
		public DateTime lastPasswordDateChanged { get; set; }

		#region Relationships with profile

		/// <summary>
		/// Профиль пользователя
		/// </summary>
		[Required]
		public virtual Profile profile { get; set; }

		#endregion

		#region Relationships with account

		/// <summary>
		/// Аккаунт пользователя
		/// </summary>
		[Required]
		public virtual Account account { get; set; }

		#endregion

		#region Relationships with friends

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		[ForeignKey("userId")]
		public virtual ICollection<User> friends { get; set; }

		#endregion

		#region Relationships with referrals

		/// <summary>
		/// Идентификатор реферера (пользователя который вас пригласил в систему)
		/// Внешний ключ
		/// </summary>
		public Guid? referrerId { get; set; }

		/// <summary>
		/// Коллекция людей, привлеченные в интернет-проект
		/// </summary>
		[JsonIgnore]
		[ForeignKey("referrerId")]
		public virtual ICollection<User> referrals { get; set; }

		#endregion

		#region Relationships with role

		/// <summary>
		/// 
		/// </summary>
		public Guid roleId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[ForeignKey("roleId")]
		public virtual Role role { get; set; }

		#endregion
	}
}