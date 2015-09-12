using System;
using System.Runtime.Serialization;

namespace ave.DTO.Customers.Memberships
{
	[DataContract]
	public class UserLoginContract : ResponseServiceObject
	{
		/// <summary>
		/// Идентификатор полльзователя
		/// </summary>
		[DataMember]
		public Guid userId { get; set; }

		/// <summary>
		/// Имя пользователя
		/// </summary>
		[DataMember]
		public string userName { get; set; }
	}
}