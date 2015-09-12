using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;

namespace ave.Service.Customers.Data.Entities
{
	[DataServiceKey("profileId")]
	public class Profile
	{
		#region Constructors
		#endregion

		/// <summary>
		/// Идентификатор профиля
		/// </summary>
		//[Key]
		[Key, ForeignKey("user")]
		public Guid profileId { get; set; }

		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string name { get; set; }

		/// <summary>
		/// Имя пользователя - видимость поля
		/// </summary>
		public int nameVisibility { get; set; }

		/// <summary>
		/// Аватарка
		/// </summary>
		public string avatar { get; set; }

		/// <summary>
		/// Пол
		/// </summary>
		public int gender { get; set; }

		/// <summary>
		/// Пол - видимость поля
		/// </summary>
		public int genderVisibility { get; set; }

		/// <summary>
		/// День рождения
		/// </summary>
		public DateTime birthdate { get; set; }

		/// <summary>
		/// День рождения - видимость поля
		/// </summary>
		public int birthdateVisibility { get; set; }

		/// <summary>
		/// Статус профиля
		/// </summary>
		public int status { get; set; }

		/// <summary>
		/// Уровень, определяет по выполнению задания или создание его
		/// </summary>
		public int level { get; set; }

		/// <summary>
		/// Карма, определяется по активности при общении в беседке или комментариях
		/// </summary>
		public int karma { get; set; }

		#region Relationships with user

		/// <summary>
		/// Пользователь
		/// </summary>
		//[JsonIgnore]
		[Required]
		public virtual User user { get; set; }

		#endregion
	}
}