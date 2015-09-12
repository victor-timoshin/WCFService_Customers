using ave.DTO.Customers.Profiles.Types;
using System;
using System.Runtime.Serialization;

namespace ave.DTO.Customers.Profiles
{
	[DataContract]
	public class ProfileEditContract
	{
		/// <summary>
		/// Идентификатор задания
		/// </summary>
		[DataMember]
		public Guid userId { get; set; }

		/// <summary>
		/// ФИО пользователя
		/// </summary>
		[DataMember]
		public string name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public FieldVisibilityTypes nameVisibility { get; set; }
	}
}