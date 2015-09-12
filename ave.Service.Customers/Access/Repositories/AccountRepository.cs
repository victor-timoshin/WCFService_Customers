using ave.DTO.Customers.Accounts.Types;
using ave.Service.Customers.Data;
using ave.Service.Customers.Data.Entities;
using ave.Service.Customers.Access.Repositories.Interfaces;
using System;
using System.Linq;

namespace ave.Service.Customers.Access.Repositories
{
	public class AccountRepository : IAccountRepository
	{
		private readonly EFServiceRepository_Helpers.IUnitOfWork unitOfWork = null;

		/// <summary>
		/// Конструктор класса по умочанию
		/// </summary>
		/// <param name="context"></param>
		public AccountRepository(DatabaseContext context)
		{
			unitOfWork = new EFServiceRepository_Helpers.UnitOfWork(context);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Create()
		{
			Account accauntEntity = new Account();
			accauntEntity.accountId = Guid.NewGuid();
			accauntEntity.balance = 0.00m;
			accauntEntity.currency = 0;
			accauntEntity.activePlan = (int)AccountTypes.Free;

			unitOfWork.RepositoryFor<Account>().Add(accauntEntity);
			unitOfWork.Commit();
		}

		/// <summary>
		/// Пополнение баланса
		/// increase
		/// </summary>
		/// <param name="account"></param>
		/// <param name="amount"></param>
		public void Replenish(Account account, decimal amount)
		{
			//TableTransaction transactionEntity = new TableTransaction();
			//transactionEntity.TransactionId = Guid.NewGuid();
			//transactionEntity.Amount = amount;
			//transactionEntity.Currency = 0;
			//transactionEntity.DateOperation = DateTime.Now;

			//m_unitOfWork.RepositoryFor<TableTransaction>().Add(transactionEntity);

			//account.Balance += amount;
			////account.Transactions = new List<TableTransaction> { transactionEntity };
			//account.Transactions.Add(transactionEntity);

			//m_unitOfWork.RepositoryFor<TableAccount>().Modified(account);
			//m_unitOfWork.Commit();
		}

		/// <summary>
		/// Списание денежных средств
		/// decrease
		/// </summary>
		/// <param name="account"></param>
		/// <param name="amount"></param>
		public void Debit(Account account, decimal amount)
		{
			//if (IsSufficient(account, amount))
			//{
			//    TableTransaction transactionEntity = new TableTransaction();
			//    transactionEntity.TransactionId = Guid.NewGuid();
			//    transactionEntity.Amount = amount;
			//    transactionEntity.Currency = 0;
			//    transactionEntity.DateOperation = DateTime.Now;

			//    m_unitOfWork.RepositoryFor<TableTransaction>().Add(transactionEntity);

			//    account.Balance -= amount;
			//    account.Transactions = new List<TableTransaction> { transactionEntity };

			//    m_unitOfWork.RepositoryFor<TableAccount>().Modified(account);
			//    m_unitOfWork.Commit();
			//}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IQueryable<Account> GetAccounts()
		{
			return unitOfWork.RepositoryFor<Account>().GetAll();
		}

		#region Private methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="account"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		private bool IsSufficient(Account account, decimal amount)
		{
			if (account.balance > amount)
			{
				return true;
			}

			return false;
		}

		#endregion
	}
}