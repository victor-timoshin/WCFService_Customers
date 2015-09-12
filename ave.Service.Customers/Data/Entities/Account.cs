using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;

namespace ave.Service.Customers.Data.Entities
{
	/// <summary>
	/// Класс лицевого счета
	/// </summary>
	[DataServiceKey("accountId")]
	public class Account
	{
		#region Constructors
		#endregion

		/// <summary>
		/// Идентификатор аккаунта
		/// </summary>
		[Key]
		public Guid accountId { get; set; }

		/// <summary>
		/// Баланс счета
		/// </summary>
		public decimal balance { get; set; }

		/// <summary>
		/// Валюта
		/// </summary>
		public int currency { get; set; }

		/// <summary>
		/// Активный план
		/// </summary>
		public int activePlan { get; set; }

		#region Relationships with user

		/// <summary>
		/// Пользователь
		/// </summary>
		[JsonIgnore]
		[Required]
		public virtual User user { get; set; }

		#endregion
	}
}