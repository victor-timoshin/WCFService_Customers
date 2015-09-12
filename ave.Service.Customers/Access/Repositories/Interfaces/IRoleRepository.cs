using ave.Service.Customers.Data.Entities;
using System.Collections.Generic;

namespace ave.Service.Customers.Access.Repositories.Interfaces
{
	public interface IRoleRepository
	{
		void Create(string roleName, string description);
		bool AttachByName(string username, string role);
		Role GetRoleByName(string roleName);
		string GetRoleByUserName(string userName);
		IEnumerable<Role> GetRoles();
		int GetTotalCount();
	}
}