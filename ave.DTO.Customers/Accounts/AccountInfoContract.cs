using System.Runtime.Serialization;

namespace ave.DTO.Customers.Accounts
{
	[DataContract]
	public class AccountInfoContract : ResponseServiceObject
	{
		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public decimal balance { get; set; }
	}
}