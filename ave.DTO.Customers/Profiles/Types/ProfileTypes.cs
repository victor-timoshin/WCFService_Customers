using System.Runtime.Serialization;

namespace ave.DTO.Customers.Profiles.Types
{
	[DataContract]
	public enum ProfileTypes
	{
		[EnumMember]
		Pending = 0,

		[EnumMember]
		Inactive = 1,

		[EnumMember]
		Active = 2,

		[EnumMember]
		Banned = 3
	}

	[DataContract]
	public enum GenderTypes
	{
		[EnumMember]
		Male = 1,

		[EnumMember]
		Female = 2,

		[EnumMember]
		Unspecified = 3
	}

	[DataContract]
	public enum FieldVisibilityTypes
	{
		/// <summary>
		/// Для всех
		/// </summary>
		[EnumMember]
		//[Description("Everyone")]
		Everyone = 1,

		/// <summary>
		/// Всем зарегистрированным пользователям
		/// </summary>
		[EnumMember]
		//[Description("AllMembers")]
		AllMembers = 2,

		/// <summary>
		/// Только друзьям
		/// </summary>
		[EnumMember]
		//[Description("MyFriends")]
		MyFriends = 3,

		/// <summary>
		/// Только мне
		/// </summary>
		[EnumMember]
		//[Description("OnlyMe")]
		OnlyMe = 4
	}
}