using ave.DTO.Customers.Accounts.Types;
using ave.DTO.Customers.Profiles.Types;
using ave.Service.Customers.Data;
using ave.Service.Customers.Data.Entities;
using ave.Service.Customers.Access.Repositories.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;

namespace ave.Service.Customers.Access.Repositories
{
	public class UserRepository : IUserRepository
	{
		private static Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);
		private readonly EFServiceRepository_Helpers.IUnitOfWork unitOfWork = null;

		/// <summary>
		/// Конструктор класса по умочанию
		/// </summary>
		/// <param name="context"></param>
		public UserRepository(DatabaseContext context)
		{
			unitOfWork = new EFServiceRepository_Helpers.UnitOfWork(context);
		}

		#region Private methods

		/// <summary>
		/// Генерируем соль для пароля
		/// </summary>
		/// <returns></returns>
		public string GetPasswordSalt()
		{
			RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
			byte[] dataArray = new byte[32];
			crypto.GetBytes(dataArray);

			return Convert.ToBase64String(dataArray);
		}

		/// <summary>
		/// Формируем хэшированный пароль
		/// </summary>
		/// <param name="password">Пароль</param>
		/// <param name="passwordSalt">Соль</param>
		/// <returns></returns>
		public string GetPasswordHash(string password, string passwordSalt)
		{
			return string.Join("", SHA1CryptoServiceProvider.Create().ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2"))).ToLower();
		}

		#endregion

		/// <summary>
		/// Создает и добавляет в базу данных нового пользователя
		/// </summary>
		/// <param name="userId">Идентификатор пользователя</param>
		/// <param name="referrerId">Идентификатор пригласившего в систему реферера</param>
		/// <param name="username">Псевдоним пользователя</param>
		/// <param name="email">Адрес электронной почты</param>
		/// <param name="password">Пароль</param>
		/// <returns></returns>
		public void Create(Guid userId, Guid? referrerId, string username, string email, string password)
		{
			#region Role

			Role roleEntity = unitOfWork.RepositoryFor<Role>().Single(i => i.name == "guest");

			#endregion

			#region Account

			Account accountEntity = new Account();
			accountEntity.accountId = Guid.NewGuid();
			accountEntity.balance = 0.00M;
			accountEntity.currency = 0;
			accountEntity.activePlan = (int)AccountTypes.Free;

			unitOfWork.RepositoryFor<Account>().Add(accountEntity);

			#endregion

			#region Profile

			Profile profileEntity = new Profile();
			profileEntity.profileId = Guid.NewGuid();
			profileEntity.name = string.Empty;
			profileEntity.nameVisibility = (int)FieldVisibilityTypes.Everyone;
			profileEntity.gender = (int)GenderTypes.Unspecified;
			profileEntity.genderVisibility = (int)FieldVisibilityTypes.Everyone;
			profileEntity.birthdate = DateTime.Now;
			profileEntity.birthdateVisibility = (int)FieldVisibilityTypes.Everyone;
			profileEntity.avatar = "/Assets/Images/noavatar.gif";
			//profileEntity.Location = locationEntity;
			profileEntity.status = (int)ProfileTypes.Active;
			profileEntity.level = 0;
			profileEntity.karma = 0;

			unitOfWork.RepositoryFor<Profile>().Add(profileEntity);
			//m_unitOfWork.Commit();

			#endregion

			#region User

			User userEntity = new User();
			userEntity.userId = userId;
			userEntity.referrerId = referrerId;
			userEntity.userName = username;
			userEntity.email = email;
			userEntity.emailKey = Guid.NewGuid().ToString();
			userEntity.passwordSalt = GetPasswordSalt();
			userEntity.password = GetPasswordHash(password, userEntity.passwordSalt);
			userEntity.isLockedOut = false;
			userEntity.dateCreated = DateTime.Now;
			userEntity.lastDateLockout = DateTime.Now; /* Дата блокировки аккаута */
			userEntity.lastDateLogin = DateTime.Now; /* Дата последнего входа */
			userEntity.lastDateModified = DateTime.Now; /* Дата последнего редактировании профиля */
			userEntity.lastPasswordDateChanged = DateTime.Now; /* Дата смены пароля */
			userEntity.account = accountEntity;
			userEntity.profile = profileEntity;
			userEntity.role = roleEntity;

			unitOfWork.RepositoryFor<User>().Add(userEntity);
			unitOfWork.Commit();

			#endregion
		}

		/// <summary>
		/// Редактирует данные пользователя
		/// </summary>
		/// <param name="model">Модель данных</param>
		public void Edit(User model)
		{
			unitOfWork.RepositoryFor<User>().Update(model);
		}

		/// <summary>
		/// Удаляет из базы данных пользователя по псевданиму
		/// </summary>
		/// <param name="username">Псевдоним пользователя</param>
		public void Delete(string username)
		{
			var userEntity = unitOfWork.RepositoryFor<User>().Single(i => i.userName == username);

			//var roleEntity = userEntity.roles.ToList();
			//roleEntity.ForEach(i => userEntity.roles.Remove(i));

			//TODO:
			//userEntity.Roles.Clear();
			//m_unitOfWork.Commit();
		}

		/// <summary>
		/// Получает пользователя по идентификатору
		/// </summary>
		/// <param name="uid">Идентификатор пользователя</param>
		/// <returns></returns>
		public User GetUserById(Guid uid)
		{
			return unitOfWork.RepositoryFor<User>().First(i => i.userId == uid);
		}

		/// <summary>
		/// Получает пользователя по имени
		/// </summary>
		/// <param name="username">Псевдоним пользователя</param>
		/// <returns></returns>
		public User GetUserByName(string username)
		{
			return unitOfWork.RepositoryFor<User>().First(i => i.userName == username);
		}

		/// <summary>
		/// Получает пользователя по адресу электронной почты
		/// </summary>
		/// <param name="email">Адрес электронной почты</param>
		/// <returns></returns>
		public User GetUserByEmail(string email)
		{
			return unitOfWork.RepositoryFor<User>().First(i => i.email == email);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pageNumber"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public IEnumerable<User> GetUsers(int pageNumber, int pageSize)
		{
			return unitOfWork.RepositoryFor<User>().GetAll().OrderByDescending(i => i.userName).Skip(pageSize * pageNumber).Take(pageSize).ToList();
		}

		/// <summary>
		/// Получает всех пользователей
		/// </summary>
		/// <returns></returns>
		public IEnumerable<User> GetUsers()
		{
			return unitOfWork.RepositoryFor<User>().GetAll();
		}

		/// <summary>
		/// Получает общее количество пользователей
		/// </summary>
		/// <returns></returns>
		public int GetTotalCount()
		{
			return unitOfWork.RepositoryFor<User>().GetAll().Count();
		}

		/// <summary>
		/// Активация пользователя
		/// </summary>
		/// <param name="username">Псевдоним пользователя</param>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ActivateUser(string username, string key)
		{
			var requestLinq = unitOfWork.RepositoryFor<User>().First(i => i.userName == username);
			if (requestLinq.emailKey == key /*&& requestLinq.status == (int)PriorityLevel.Pending*/)
			{
				//requestLinq.status = (int)PriorityLevel.Inactive;
				requestLinq.lastDateModified = DateTime.Now;

				unitOfWork.Commit();
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Проверяем на валидность входные данные
		/// </summary>
		/// <param name="username">Псевдоним пользователя</param>
		/// <param name="password">Пароль</param>
		/// <returns></returns>
		public bool ValidateUser(string username, string password)
		{
			var account = unitOfWork.RepositoryFor<User>().First(i => i.userName == username);
			if (account != null && account.password == GetPasswordHash(password, account.passwordSalt)/* && account.status != (int)PriorityLevel.Pending*/)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Изменяет старый пароль на новый
		/// </summary>
		/// <param name="username">Имя пользователя для которого следует изменить пароль</param>
		/// <param name="oldPassword">Старый пароль</param>
		/// <param name="newPassword">Новый пароль</param>
		/// <returns>true - если смена пороля прошла успешно, в противном случае false</returns>
		public bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			if (ValidateUser(username, oldPassword) && !String.IsNullOrEmpty(newPassword.Trim()))
			{
				var user = unitOfWork.RepositoryFor<User>().First(i => i.userName == username);

				user.passwordSalt = GetPasswordSalt();
				user.password = GetPasswordHash(newPassword, user.passwordSalt);

				unitOfWork.RepositoryFor<User>().Update(user);
				unitOfWork.Commit();
				return true;
			}

			return false;
		}

		#region User providers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="uid">Уникальный идентификатор пользователя</param>
		/// <param name="providerName">Имя поставщика</param>
		/// <param name="providerPageId">Уникальный идентификатор страницы пользователя в социальной сети</param>
		public void CreateUserProvider(Guid uid, string providerName, string providerPageId)
		{
			UserProvider providerEntity = new UserProvider();
			providerEntity.providerId = Guid.NewGuid();
			providerEntity.userId = uid;
			providerEntity.providerName = providerName;
			providerEntity.providerPageId = providerPageId;

			unitOfWork.RepositoryFor<UserProvider>().Add(providerEntity);
			unitOfWork.Commit();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		public IEnumerable<UserProvider> GetProvidersByUserId(Guid uid)
		{
			return unitOfWork.RepositoryFor<UserProvider>().Find(i => i.userId == uid);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="providerName">Имя поставщика</param>
		/// <returns></returns>
		public IEnumerable<UserProvider> GetProvidersByName(string providerName)
		{
			return unitOfWork.RepositoryFor<UserProvider>().Find(i => i.providerName == providerName);
		}

		#endregion

		#region Friends

		///// <summary>
		///// Добавляет в друзья
		///// </summary>
		///// <param name="uid">Идентификатор пользователя</param>
		///// <param name="friendId">Идентификатор друга пользователя</param>
		//public void AddFriend(Guid uid, Guid friendId)
		//{
		//    TableUser user1 = m_unitOfWork.RepositoryFor<TableUser>().Single(i => i.ID == uid);
		//    TableUser user2 = m_unitOfWork.RepositoryFor<TableUser>().Single(i => i.ID == friendId);

		//    user1.Friends.Add(user2);
		//    m_unitOfWork.RepositoryFor<TableUser>().Attach(user1);

		//    user2.Friends.Add(user1);
		//    m_unitOfWork.RepositoryFor<TableUser>().Attach(user2);

		//    m_unitOfWork.Commit();
		//}

		///// <summary>
		///// Удаляет из друзей
		///// </summary>
		///// <param name="uid">Идентификатор пользователя</param>
		///// <param name="friendId">Идентификатор друга пользователя</param>
		//public void DeleteFriend(Guid uid, Guid friendId)
		//{
		//    TableUser user1 = m_unitOfWork.RepositoryFor<TableUser>().Single(i => i.ID == uid);
		//    TableUser user2 = m_unitOfWork.RepositoryFor<TableUser>().Single(i => i.ID == friendId);

		//    if (user1.Friends.Contains(user2))
		//    {
		//        user1.Friends.Remove(user2);
		//        m_unitOfWork.RepositoryFor<TableUser>().Delete(user1);
		//    }

		//    if (user2.Friends.Contains(user1))
		//    {
		//        user2.Friends.Remove(user1);
		//        m_unitOfWork.RepositoryFor<TableUser>().Delete(user2);
		//    }

		//    m_unitOfWork.Commit();
		//}

		///// <summary>
		///// Получает всех друзей
		///// </summary>
		///// <param name="uid">Идентификатор пользователя</param>
		///// <returns></returns>
		//public IQueryable<TableUser> GetFriends(Guid uid)
		//{
		//    return m_unitOfWork.RepositoryFor<TableUser>().Single(i => i.ID == uid).Friends.AsQueryable();
		//}

		#endregion

		#region Friend requests

		///// <summary>
		///// Создает новый запрос на дружбу
		///// </summary>
		///// <param name="from"></param>
		///// <param name="to"></param>
		//public void CreateFriendRequest(Guid from, Guid to)
		//{
		//    TableFriendRequest friendRequest = new TableFriendRequest();
		//    friendRequest.RequestId = Guid.NewGuid();
		//    friendRequest.SenderId = from;
		//    friendRequest.ReceiverId = to;

		//    m_unitOfWork.RepositoryFor<TableFriendRequest>().Add(friendRequest);
		//    m_unitOfWork.Commit();
		//}

		///// <summary>
		///// Удаляет существующий запрос
		///// </summary>
		///// <param name="friendRequest"></param>
		//public void DeleteFriendRequest(TableFriendRequest friendRequest)
		//{
		//    m_unitOfWork.RepositoryFor<TableFriendRequest>().Delete(friendRequest);
		//    m_unitOfWork.Commit();
		//}

		///// <summary>
		///// Получает все заявки в друзья (входящие и исходящие)
		///// </summary>
		///// <returns></returns>
		//public IQueryable<TableFriendRequest> GetAllRequestsToFriends()
		//{
		//    return m_unitOfWork.RepositoryFor<TableFriendRequest>().GetAll();
		//}

		///// <summary>
		///// Получает входящие заявки в Друзья
		///// </summary>
		///// <param name="uid"></param>
		///// <returns></returns>
		//public IQueryable<TableFriendRequest> GetIncomingRequestsToFriends(Guid uid)
		//{
		//    return m_unitOfWork.RepositoryFor<TableFriendRequest>().Find(i => i.ReceiverId == uid);
		//}

		///// <summary>
		///// Получает исходящие заявки в Друзья
		///// </summary>
		///// <param name="uid"></param>
		///// <returns></returns>
		//public IQueryable<TableFriendRequest> GetOutgoingRequestsToFriends(Guid uid)
		//{
		//    return m_unitOfWork.RepositoryFor<TableFriendRequest>().Find(i => i.SenderId == uid);
		//}

		///// <summary>
		///// Проверяет, отправил ли пользователь (from) пользователю (to) запрос на дружбу
		///// </summary>
		///// <param name="from"></param>
		///// <param name="to"></param>
		//public bool IsRequestToFriends(Guid from, Guid to)
		//{
		//    return m_unitOfWork.RepositoryFor<TableFriendRequest>().Find(i => i.SenderId == from).Count(i => i.ReceiverId == to) != 0;
		//}

		#endregion
	}
}