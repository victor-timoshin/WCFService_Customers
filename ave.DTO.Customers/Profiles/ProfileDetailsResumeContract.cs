using System;
using System.Runtime.Serialization;

namespace ave.DTO.Customers.Profiles
{
	[DataContract]
	public class ProfileDetailsResumeContract
	{
		/// <summary>
		/// Идентификатор задания
		/// </summary>
		[DataMember]
		public Guid userId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public string email { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public int age { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public int gender { get; set; }
	}
}