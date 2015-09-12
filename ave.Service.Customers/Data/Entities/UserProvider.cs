using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ave.Service.Customers.Data.Entities
{
	public class UserProvider
	{
		/// <summary>
		/// Идентификатор поставщика
		/// </summary>
		public Guid providerId { get; set; }

		/// <summary>
		/// Имя поставщика
		/// </summary>
		public string providerName { get; set; }

		/// <summary>
		/// Идентификатор страницы пользователя в другой системе
		/// </summary>
		public string providerPageId { get; set; }

		/// <summary>
		/// Идентификатор пользователя
		/// </summary>
		public Guid userId { get; set; }
	}
}