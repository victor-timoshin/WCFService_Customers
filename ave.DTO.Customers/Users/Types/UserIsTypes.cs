using System.Runtime.Serialization;

namespace ave.DTO.Customers.Users.Types
{
	[DataContract]
	public enum UserIsTypes
	{
		[EnumMember]
		Me = 1,

		[EnumMember]
		Friend = 2,

		[EnumMember]
		Referrer = 3,

		[EnumMember]
		Referral = 4
	}
}