using System.Runtime.Serialization;

namespace ave.DTO.Customers.Memberships
{
	[DataContract]
	public class RegisterContract : ResponseServiceObject
	{
		/// <summary>
		/// Идентификатор сессии
		/// </summary>
		[DataMember]
		public string sessionId { get; set; }
	}
}