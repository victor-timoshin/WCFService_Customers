using System.Runtime.Serialization;

namespace ave.DTO.Customers.Accounts.Types
{
	[DataContract]
	public enum AccountTypes
	{
		[EnumMember]
		Free = 0,

		[EnumMember]
		Silver = 1,

		[EnumMember]
		Bronze = 2,

		[EnumMember]
		Gold = 3
	}
}