using System;
using System.Runtime.Serialization;

namespace ave.DTO.Customers.Roles
{
	[DataContract(IsReference = false)]
	public class RoleListContract
	{
		/// <summary>
		/// Идентификатор роли
		/// </summary>
		[DataMember]
		public Guid roleId { get; set; }

		/// <summary>
		/// Имя роли
		/// </summary>
		[DataMember]
		public string name { get; set; }

		/// <summary>
		/// Краткое описание
		/// </summary>
		[DataMember]
		public string description { get; set; }
	}
}