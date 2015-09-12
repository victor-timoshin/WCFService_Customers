using ave.DTO.Customers.Users.Types;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ave.DTO.Customers.Users
{
	[DataContract]
	public class UserDetailsContract : ResponseServiceObject
	{
		/// <summary>
		/// Идентификатор пользователя
		/// </summary>
		[DataMember]
		public Guid userId { get; set; }

		/// <summary>
		/// Псевдоним
		/// </summary>
		[DataMember]
		public string userName { get; set; }

		/// <summary>
		/// Пользователь является
		/// </summary>
		[DataMember]
		public ICollection<UserIsTypes> userIs { get; set; }

		/// <summary>
		/// я уже отправил заявку в друзья этому пользователю
		/// </summary>
		[DataMember]
		public bool friendRequestIsSend { get; set; }
	}
}