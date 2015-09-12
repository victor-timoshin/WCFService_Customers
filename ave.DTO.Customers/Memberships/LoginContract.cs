using System.Runtime.Serialization;

namespace ave.DTO.Customers.Memberships
{
	[DataContract]
	public class LoginContract : ResponseServiceObject
	{
		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public UserLoginContract user { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public string roleName { get; set; }
	}
}