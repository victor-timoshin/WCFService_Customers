using ave.Service.Customers.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ave.Service.Customers.Access.Repositories.Interfaces
{
	public interface IUserRepository
	{
		void Create(Guid userId, Guid? referrerId, string userName, string email, string password);
		void Edit(User model);
		void Delete(string userName);
		User GetUserById(Guid uid);
		User GetUserByName(string userName);
		User GetUserByEmail(string email);
		IEnumerable<User> GetUsers(int pageNumber, int pageSize);
		IEnumerable<User> GetUsers();
		int GetTotalCount();
		bool ActivateUser(string userName, string key);
		bool ValidateUser(string userName, string password);
		bool ChangePassword(string userName, string oldPassword, string newPassword);

		#region User providers

		void CreateUserProvider(Guid uid, string providerName, string providerPageId);
		IEnumerable<UserProvider> GetProvidersByUserId(Guid uid);
		IEnumerable<UserProvider> GetProvidersByName(string providerName);

		#endregion

		#region Friends

		//void AddFriend(Guid uid, Guid friendId);
		//void DeleteFriend(Guid uid, Guid friendId);
		//IQueryable<User> GetFriends(Guid uid);

		#endregion

		#region Friend requests

		//void CreateFriendRequest(Guid from, Guid to);
		//void DeleteFriendRequest(FriendRequest friendRequest);
		//IQueryable<FriendRequest> GetAllRequestsToFriends();
		//IQueryable<FriendRequest> GetIncomingRequestsToFriends(Guid uid);
		//IQueryable<FriendRequest> GetOutgoingRequestsToFriends(Guid uid);
		//bool IsRequestToFriends(Guid from, Guid to);

		#endregion
	}
}