using System;
using System.Runtime.Serialization;

namespace ave.DTO.Customers.Users
{
	[DataContract]
	public class UserLinkContract : ResponseServiceObject
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
		public string fullName { get; set; }

		/// <summary>
		/// Аватар пользователя
		/// </summary>
		[DataMember]
		public string avatar { get; set; }
	}
}