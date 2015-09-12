using ave.Service.Customers.Data.Entities;
using System.Linq;

namespace ave.Service.Customers.Access.Repositories.Interfaces
{
	public interface IAccountRepository
	{
		void Create();
		void Replenish(Account account, decimal amount);
		void Debit(Account account, decimal amount);
		IQueryable<Account> GetAccounts();
	}
}