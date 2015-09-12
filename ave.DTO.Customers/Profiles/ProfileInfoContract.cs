using System;
using System.Runtime.Serialization;

namespace ave.DTO.Customers.Profiles
{
	[DataContract]
	public class ProfileInfoContract : ResponseServiceObject
	{
		/// <summary>
		/// Идентификатор пользователя
		/// </summary>
		[DataMember]
		public Guid userId { get; set; }

		/// <summary>
		/// Псевдоним пользователя
		/// </summary>
		[DataMember]
		public string userName { get; set; }

		/// <summary>
		/// ФИО пользователя
		/// </summary>
		[DataMember]
		public string name { get; set; }

		/// <summary>
		/// Адрес электронной почты
		/// </summary>
		[DataMember]
		public string email { get; set; }

		/// <summary>
		/// Дата создания профиля
		/// </summary>
		[DataMember]
		public DateTime dateCreated { get; set; }

		/// <summary>
		/// Аватар
		/// </summary>
		[DataMember]
		public string avatar { get; set; }

		/// <summary>
		/// Карма
		/// </summary>
		[DataMember]
		public int karma { get; set; }
	}
}