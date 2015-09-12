using System.Runtime.Serialization;

namespace ave.DTO.Customers
{
	[DataContract]
	public class ResponseServiceObject
	{
		[DataMember]
		public ResponseService response { get; set; }
	}
}