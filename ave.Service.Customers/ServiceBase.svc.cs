using ave.DTO.Customers;
using ave.DTO.Customers.Accounts;
using ave.DTO.Customers.Memberships;
using ave.DTO.Customers.Profiles;
using ave.DTO.Customers.Profiles.Types;
using ave.DTO.Customers.Roles;
using ave.DTO.Customers.Users;
using ave.DTO.Customers.Users.Types;
using ave.Service.Customers.Data;
using ave.Service.Customers.Data.Entities;
using ave.Service.Customers.Access.Repositories;
using ave.Service.Customers.Access.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace ave.Service.Customers
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class ServiceBase : IServiceBase
	{
		private IRoleRepository roleRepository = null;
		private IUserRepository userRepository = null;
		private IAccountRepository accountRepository = null;
		private IProfileRepository profileRepository = null;

		/// <summary>
		/// Конструктор класса
		/// </summary>
		public ServiceBase()
		{
			var context = new DatabaseContext();

			roleRepository = new RoleRepository(context);
			userRepository = new UserRepository(context);
			accountRepository = new AccountRepository(context);
			profileRepository = new ProfileRepository(context);
		}

		public void TestUsers()
		{
			var testUsername = "test_user_{0}";
			var testEmail = "test_user_{0}@mail.ru";
			var testPassword = "demo123";

			/* Администратор */
			userRepository.Create(Guid.Parse("e0ae3253-2c40-46a2-bcb6-2552e713f203"), null, string.Format(testUsername, "admin"), string.Format(testEmail, "admin"), testPassword);
			roleRepository.AttachByName(string.Format(testUsername, "admin"), "admin");

			/* Агенты */
			userRepository.Create(Guid.Parse("a41ba7b6-2daf-45fe-a311-433c85ba8b7a"), null, string.Format(testUsername, "agent1"), string.Format(testEmail, "agent1"), testPassword);
			userRepository.Create(Guid.Parse("66ff1a0a-34a4-4fce-8f7f-af6ab5acf096"), null, string.Format(testUsername, "agent2"), string.Format(testEmail, "agent2"), testPassword);

			/* Пользователи */
			for (int i = 0; i < 10; i++)
			{
				userRepository.Create(Guid.NewGuid(), null, string.Format(testUsername, i), string.Format(testEmail, i), testPassword);
			}
		}

		#region Memberships

		/// <summary>
		/// Регистрация нового пользователя
		/// </summary>
		/// <param name="reflink">Имя реферера</param>
		/// <param name="username">Псевдоним пользователя</param>
		/// <param name="email">Адрес электронной почты</param>
		/// <param name="password">Пароль</param>
		/// <returns></returns>
		public RegisterContract CreateUserAndAccount(string reflink, string username, string email, string password)
		{
			String sessionId = String.Empty;
			RegisterContract result = null;

			try
			{
				result = new RegisterContract();
				result.response = new ResponseService();
				result.response.statusCode = 0;
				result.response.statusDescription = string.Empty;

				/* Проверяем наличие имени в базе данных */
				if (userRepository.GetUserByName(username) != null)
				{
					result.response.statusCode = 449;
					result.response.statusDescription = "Retry With";
					return result;
				}

				/* Проверяем наличие адреса электронной почты в базе данных */
				if (userRepository.GetUserByEmail(email) != null)
				{
					result.response.statusCode = 418;
					result.response.statusDescription = "I'm a teapot";
					return result;
				}

				Guid? referrerId = null;
				User referrer = userRepository.GetUserByName(reflink);
				if (referrer != null)
					referrerId = referrer.userId;

				userRepository.Create(Guid.NewGuid(), referrerId, username, email, password);

				result.response.statusCode = 201;
				result.response.statusDescription = "Created";
			}
			catch (System.Exception ex)
			{
				result.response.statusDescription = ex.Message;
				result.response.statusCode = 500;
			}
			finally
			{
				result.sessionId = sessionId;
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="username">Псевдоним пользователя</param>
		/// <param name="password">Пароль</param>
		/// <returns></returns>
		public LoginContract Login(string username, string password)
		{
			LoginContract result = null;

			try
			{
				result = new LoginContract();
				result.response = new ResponseService();
				result.response.statusCode = 0;
				result.response.statusDescription = string.Empty;

				User user = userRepository.GetUserByName(username);
				if (user == null)
				{
					result.response.statusCode = 401;
					result.response.statusDescription = "Unauthorized";
					return result;
				}

				if (userRepository.ValidateUser(username, password))
				{
					result.user = new UserLoginContract { userId = user.userId, userName = user.userName };
					result.roleName = roleRepository.GetRoleByUserName(username);
					result.response.statusCode = 202;
					result.response.statusDescription = "Accepted";
					return result;
				}
				else
				{
					result.response.statusCode = 412;
					result.response.statusDescription = "Precondition Failed";
					return result;
				}
			}
			catch (System.Exception ex)
			{
				result.response.statusDescription = ex.Message;
				result.response.statusCode = 500;
			}
			finally
			{
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Logout()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsAlive()
		{
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="username">Псевдоним пользователя</param>
		/// <param name="key"></param>
		public void Activate(string username, string key)
		{
			userRepository.ActivateUser(username, key);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userName">Псевдоним пользователя</param>
		/// <param name="oldPassword">Старый пароль</param>
		/// <param name="newPassword">Новый пароль</param>
		/// <returns></returns>
		public ChangePasswordContract ChangePassword(string userName, string oldPassword, string newPassword)
		{
			ChangePasswordContract result = null;

			try
			{
				result = new ChangePasswordContract();
				result.response = new ResponseService();
				result.response.statusCode = 0;
				result.response.statusDescription = string.Empty;

				bool isChanged = userRepository.ChangePassword(userName, oldPassword, newPassword);
				if (isChanged)
				{
					result.response.statusCode = 202;
					result.response.statusDescription = "Accepted";
					return result;
				}
				else
				{
					result.response.statusCode = 412;
					result.response.statusDescription = "Precondition Faile";
					return result;
				}
			}
			catch (System.Exception ex)
			{
				result.response.statusDescription = ex.Message;
				result.response.statusCode = 500;
			}
			finally
			{
			}

			return result;
		}

		#endregion

		#region Accounts

		/// <summary>
		///
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public AccountInfoContract GetBalanceByUserId(Guid uid)
		{
			Account accountEntity = userRepository.GetUserById(uid).account;
			return new AccountInfoContract
			{
				balance = accountEntity.balance,
				response = new ResponseService
				{
					statusCode = 200,
					statusDescription = "OK"
				}
			};
		}

		/// <summary>
		/// check available balance
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		public ResponseService CheckBalance(Guid uid)
		{
			decimal taskCost = 5;
			Account account = userRepository.GetUserById(uid).account;

			ResponseService result = new ResponseService();
			if (account.balance < taskCost)
			{
				result.statusCode = 500;
				result.statusDescription = "failed";
			}
			else
			{
				result.statusCode = 200;
				result.statusDescription = "ok";
			}

			return result;
		}

		/// <summary>
		/// Пополняет баланса счета
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="value"></param>
		public ResponseServiceObject Replenish(Guid uid, decimal value)
		{
			decimal interestRate = Decimal.Round((value / 100m) * 0.1m, 2); /* высчитываем процент(0.1%) вводимой суммы и округляем */
			decimal money = value - interestRate;

			User userEntity = userRepository.GetUserById(uid);
			accountRepository.Replenish(userEntity.account, money);

			return new ResponseServiceObject
			{
				response = new ResponseService
				{
					statusCode = 200,
					statusDescription = "OK"
				}
			};
		}

		/// <summary>
		/// Списываем денежные средства
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="value"></param>
		public ResponseServiceObject Debit(Guid uid, decimal value)
		{
			decimal interestRate = Decimal.Round((value / 100m) * 0.8m, 2); /* высчитываем процент(0.8%) вводимой суммы и округляем */
			decimal money = value - interestRate;

			Account accountEntity = userRepository.GetUserById(uid).account;
			accountRepository.Debit(accountEntity, money);

			return new ResponseServiceObject
			{
				response = new ResponseService
				{
					statusCode = 200,
					statusDescription = "OK"
				}
			};
		}

		#endregion

		#region Profiles

		/// <summary>
		/// Получаем все профили пользователей без реферера (свободных)
		/// </summary>
		/// <returns></returns>
		public IQueryable<ProfileInfoContract> GetFreeReferrals()
		{
			IList<ProfileInfoContract> profiles = new List<ProfileInfoContract>();
			foreach (var item in userRepository.GetUsers().Where(i => i.referrerId == null))
			{
				profiles.Add(new ProfileInfoContract
				{
					userId = item.userId,
					userName = item.userName,
					name = item.profile.name,
					email = item.email,
					avatar = item.profile.avatar,
					dateCreated = DateTime.Now,
					karma = item.profile.karma,
					response = new ResponseService
					{
						statusCode = 0,
						statusDescription = string.Empty
					}
				});
			}

			return profiles.AsQueryable();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ProfileInfoContract GetProfileInfoById(string id)
		{
			var user = userRepository.GetUserById(new Guid(id));
			return new ProfileInfoContract
			{
				userId = user.userId,
				userName = user.userName,
				name = user.profile.name,
				email = user.email,
				avatar = user.profile.avatar,
				dateCreated = DateTime.Now,
				karma = user.profile.karma
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ProfileInfoContract GetProfileInfoByName(string name)
		{
			var response = new ResponseService();
			response.statusCode = 0;
			response.statusDescription = string.Empty;

			var user = userRepository.GetUserByName(name);
			if (user != null)
			{
				response.statusCode = 200;
				response.statusDescription = "OK";
			}
			else
			{
				response.statusCode = 404;
				response.statusDescription = "Not Found";
			}

			return new ProfileInfoContract
			{
				userId = user.userId,
				userName = user.userName,
				name = user.profile.name,
				email = user.email,
				avatar = user.profile.avatar,
				dateCreated = DateTime.Now,
				karma = user.profile.karma,
				response = response
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		public Stream GetCustomerId(string uid, string current_uid)
		{
			var user = userRepository.GetUserById(new Guid(uid));
			var currentUser = userRepository.GetUserById(new Guid(current_uid));

			List<UserIsTypes> userIs = new List<UserIsTypes>();
			if (uid.Contains(current_uid))
			{
				userIs.Add(UserIsTypes.Me);
			}
			else
			{
				if (user.friends.Select(i => i.userId == currentUser.userId).Count() > 0)
				{
					userIs.Add(UserIsTypes.Friend);
				}

				if (user.referrals.Select(i => i.userId == currentUser.userId).Count() > 0)
				{
					userIs.Add(UserIsTypes.Referrer);
				}
				else if (user.referrerId == currentUser.userId)
				{
					userIs.Add(UserIsTypes.Referral);
				}
			}

			dynamic dynamicObject = new ExpandoObject();

			dynamicObject.Is = new List<UserIsTypes>(userIs);
			dynamicObject.FriendRequestIsSend = false;

			//dynamicObject.AccountInfo = new ExpandoObject();
			//dynamicObject.AccountInfo.Balance = user.Account.Balance;

			dynamicObject.ProfileInfo = new ExpandoObject();
			dynamicObject.ProfileInfo.UserId = user.userId;
			dynamicObject.ProfileInfo.UserName = user.userName;
			dynamicObject.ProfileInfo.Name = user.profile.name;
			dynamicObject.ProfileInfo.Email = user.email;
			dynamicObject.ProfileInfo.Avatar = user.profile.avatar;
			dynamicObject.ProfileInfo.DateCreated = DateTime.Now;
			dynamicObject.ProfileInfo.Karma = user.profile.karma;

			return WriteJson(dynamicObject);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		public ProfileDetailsResumeContract GetProfileDetailsResume(Guid uid)
		{
			var userEntity = userRepository.GetUserById(uid);

			var contract = new ProfileDetailsResumeContract();
			contract.userId = userEntity.userId;
			contract.email = userEntity.email;

			int age = DateTime.Today.Year - userEntity.profile.birthdate.Year;
			if (age > 0)
			{
				age -= Convert.ToInt32(DateTime.Today.Date < userEntity.profile.birthdate.Date.AddYears(age));
			}
			else
			{
				age = 0;
			}

			contract.age = age;
			contract.gender = userEntity.profile.gender;

			return contract;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		public void EditProfile(ProfileEditContract model)
		{
			profileRepository.Edit(model.userId, model.name.Trim(), model.nameVisibility, 1, DateTime.Now);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		public ProfileEditContract GetEditProfile(Guid uid)
		{
			User userEntity = userRepository.GetUserById(uid);

			ProfileEditContract contract = new ProfileEditContract();
			contract.userId = uid;
			contract.name = userEntity.profile.name;
			contract.nameVisibility = (FieldVisibilityTypes)Enum.ToObject(typeof(FieldVisibilityTypes), userEntity.profile.nameVisibility);

			return contract;
		}

		#endregion

		#region Users

		/// <summary>
		/// Получает всех пользователей из базы данных
		/// </summary>
		/// <returns>Возвращает коллекцию пользователей</returns>
		public IQueryable<UserContract> GetUsers()
		{
			List<UserContract> userContracts = new List<UserContract>();
			foreach (var item in userRepository.GetUsers())
			{
				userContracts.Add(new UserContract
				{
					userId = item.userId,
					userName = item.userName,
					email = item.email,
					isLockedOut = item.isLockedOut,
					dateCreated = item.dateCreated,
					lastDateLogin = item.lastDateLogin,
					lastDateModified = item.lastDateModified,
					lastDateLockout = item.lastDateLockout,
					lastPasswordDateChanged = item.lastPasswordDateChanged,
					referrerId = item.referrerId ?? Guid.Empty

				});
			}

			return userContracts.AsQueryable();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		public UserContract GetUserById(Guid uid)
		{
			var userEntity = userRepository.GetUserById(uid);
			return new UserContract
			{
				userId = userEntity.userId,
				userName = userEntity.userName,
				email = userEntity.email,
				isLockedOut = userEntity.isLockedOut,
				dateCreated = userEntity.dateCreated,
				lastDateLogin = userEntity.lastDateLogin,
				lastDateModified = userEntity.lastDateModified,
				lastDateLockout = userEntity.lastDateLockout,
				lastPasswordDateChanged = userEntity.lastPasswordDateChanged,
				referrerId = userEntity.referrerId ?? Guid.Empty
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public User GetUserByName(string name)
		{
			return userRepository.GetUserByName(name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		public UserDetailsContract DetailsUser(string uid, string current)
		{
			User user = userRepository.GetUserById(new Guid(uid));
			User currentUser = userRepository.GetUserById(new Guid(current));

			ICollection<UserIsTypes> userIs = new List<UserIsTypes>();
			if (uid.Contains(current))
			{
				userIs.Add(UserIsTypes.Me);
			}
			else
			{
				if (user.friends.Select(i => i.userId == currentUser.userId).Count() > 0)
				{
					userIs.Add(UserIsTypes.Friend);
				}

				if (user.referrals.Select(i => i.userId == currentUser.userId).Count() > 0)
				{
					userIs.Add(UserIsTypes.Referrer);
				}
				else if (user.referrerId == currentUser.userId)
				{
					userIs.Add(UserIsTypes.Referral);
				}
			}

			return new UserDetailsContract
			{
				userId = user.userId,
				userName = user.userName,
				userIs = userIs,
				friendRequestIsSend = false
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="nickname"></param>
		/// <param name="lastDateModified"></param>
		public void EditUser(string uid, string nickname, string lastDateModified)
		{
			User userEntity = userRepository.GetUserById(new Guid(uid));
			userEntity.userName = nickname.Trim();
			userEntity.lastDateModified = DateTime.Parse(lastDateModified);
			userRepository.Edit(userEntity);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public IQueryable<UserLinkContract> FindUser(string name)
		{
			IQueryable<Profile> profiles = profileRepository.FindProfileByFullName(name);

			IList<UserLinkContract> users = new List<UserLinkContract>();
			foreach (var item in profiles)
			{
				users.Add(new UserLinkContract
				{
					userId = Guid.NewGuid(),
					fullName = item.name,
					avatar = item.avatar
				});
			}

			return users.AsQueryable();
		}

		#endregion

		#region User providers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="providerName">Имя поставщика пользователя</param>
		/// <param name="providerPageId">Уникальный идентификатор страницы пользователя в социальной сети</param>
		/// <param name="username">Имя пользователя</param>
		/// <returns></returns>
		public string CreateOrUpdateAccount(string providerName, string providerPageId, string username)
		{
			User userEntity = userRepository.GetUserByName(username);
			userRepository.CreateUserProvider(userEntity.userId, providerName, providerPageId);

			return "result";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="providerName">Имя поставщика пользователя</param>
		/// <param name="providerPageId">Уникальный идентификатор страницы пользователя в социальной сети</param>
		/// <returns></returns>
		public LoginContract LoginExternal(string providerName, string providerPageId)
		{
			LoginContract result = null;

			try
			{
				result = new LoginContract();
				result.response = new ResponseService();
				result.response.statusCode = 0;
				result.response.statusDescription = "OK";

				var userProvider = userRepository.GetProvidersByName(providerName).FirstOrDefault(i => i.providerPageId == providerPageId);
				var user = userRepository.GetUserById(userProvider.userId);
				if (user == null)
				{
					result.response.statusCode = 1001;
					result.response.statusDescription = "A user with this name already exists";
					return result;
				}
				else
				{
					result.user = new UserLoginContract { userId = user.userId, userName = user.userName };
					result.roleName = roleRepository.GetRoleByUserName(user.userName);
					result.response.statusDescription = "успешный вход в систему";
					return result;
				}
			}
			catch (System.Exception ex)
			{
				result.response.statusDescription = ex.Message;
				result.response.statusCode = 1002;
			}
			finally
			{
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		public IQueryable<UserProvider> GetUserProviders(Guid uid)
		{
			IEnumerable<UserProvider> userProviders = userRepository.GetProvidersByUserId(uid);
			return userProviders.AsQueryable();
		}

		#endregion

		#region Roles

		/// <summary>
		/// Полуает все роли из базы данных
		/// </summary>
		/// <returns>Возвращает коллекцию ролей</returns>
		public IQueryable<RoleListContract> GetRoles()
		{
			var roles = new List<RoleListContract>();
			foreach (var item in roleRepository.GetRoles())
			{
				roles.Add(new RoleListContract
				{
					roleId = item.roleId,
					name = item.name,
					description = item.description
				});
			}

			return roles.AsQueryable();
		}

		#endregion

		#region Private methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private Stream WriteJson(object value)
		{
			var json = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));

			var memoryStream = new MemoryStream(json);
			WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
			return memoryStream;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		private object ReadJson(Stream stream)
		{
			try
			{
				StreamReader reader = new StreamReader(stream);
				string jsonData = reader.ReadToEnd();

				JavaScriptSerializer serializer = new JavaScriptSerializer();
				return serializer.Deserialize<object>(jsonData);
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		#endregion
	}
}
