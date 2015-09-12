using System.Runtime.Serialization;

namespace ave.DTO.Customers
{
	[DataContract]
	public class ResponseService
	{
		/// <summary>
		/// Код ошибки
		/// </summary>
		[DataMember]
		public int statusCode { get; set; }

		/// <summary>
		/// Сообщение описывающее ошибку
		/// </summary>
		[DataMember]
		public string statusDescription { get; set; }
	}
}