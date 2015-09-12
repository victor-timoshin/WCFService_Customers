using System;
using System.Runtime.Serialization;

namespace ave.DTO.Customers.Users
{
	[DataContract]
	public class UserContract : ResponseServiceObject
	{
		/// <summary>
		/// Идентификатор пользователя
		/// </summary>
		[DataMember]
		public Guid userId { get; set; }

		///// <summary>
		///// Аватарка пользователя
		///// </summary>
		//public string avatar { get; set; }

		/// <summary>
		/// Псевдоним
		/// </summary>
		[DataMember]
		public string userName { get; set; }

		/// <summary>
		/// Адрес электронной почты
		/// </summary>
		[DataMember]
		public string email { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public bool isLockedOut { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public DateTime dateCreated { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public DateTime lastDateLogin { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public DateTime lastDateModified { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public DateTime lastDateLockout { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public DateTime lastPasswordDateChanged { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public Guid referrerId { get; set; }
	}
}