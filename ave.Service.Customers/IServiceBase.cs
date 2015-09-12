using ave.DTO.Customers;
using ave.DTO.Customers.Accounts;
using ave.DTO.Customers.Memberships;
using ave.DTO.Customers.Profiles;
using ave.DTO.Customers.Roles;
using ave.DTO.Customers.Users;
using ave.Service.Customers.Data.Entities;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace ave.Service.Customers
{
	/// <summary>
	/// 
	/// </summary>
	[ServiceContract]
	public interface IServiceBase
	{
		[OperationContract]
		[WebGet(UriTemplate = "testusers", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
		void TestUsers();

		#region Memberships

		/// <summary>
		/// 
		/// dynamic format
		/// </summary>
		/// <param name="reflink"></param>
		/// <param name="username"></param>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <param name="passwordQuestion"></param>
		/// <param name="passwordAnswer"></param>
		/// <returns></returns>
		[OperationContract]
		[WebGet(UriTemplate = "memberships/create?reflink={reflink}&username={username}&email={email}&password={password}", ResponseFormat = WebMessageFormat.Json)]
		[Description("Creates a new user")]
		RegisterContract CreateUserAndAccount(string reflink, string username, string email, string password);

		[OperationContract]
		[WebGet(UriTemplate = "memberships/login?username={username}&password={password}", ResponseFormat = WebMessageFormat.Json)]
		LoginContract Login(string username, string password);

		[OperationContract]
		[WebGet(UriTemplate = "memberships/logout")]
		void Logout();

		[OperationContract]
		[WebGet(UriTemplate = "memberships/activate?username={username}&key={key}")]
		void Activate(string username, string key);

		[OperationContract]
		[WebGet(UriTemplate = "memberships/changepassword?userName={userName}&oldPassword={oldPassword}&newPassword={newPassword}", ResponseFormat = WebMessageFormat.Json)]
		ChangePasswordContract ChangePassword(string userName, string oldPassword, string newPassword);

		[OperationContract]
		[WebGet(UriTemplate = "isalive")]
		bool IsAlive();

		#endregion

		#region Accounts

		[OperationContract]
		[WebGet(UriTemplate = "accounts/balance?uid={uid}", ResponseFormat = WebMessageFormat.Json)]
		AccountInfoContract GetBalanceByUserId(Guid uid);

		[OperationContract]
		[WebGet(UriTemplate = "accounts/check?uid={uid}", ResponseFormat = WebMessageFormat.Json)]
		ResponseService CheckBalance(Guid uid);

		[OperationContract]
		[WebGet(UriTemplate = "replenish?uid={uid}&value={value}", ResponseFormat = WebMessageFormat.Json)]
		ResponseServiceObject Replenish(Guid uid, decimal value);

		[OperationContract]
		[WebGet(UriTemplate = "debit?uid={uid}&value={value}", ResponseFormat = WebMessageFormat.Json)]
		ResponseServiceObject Debit(Guid uid, decimal value);

		#endregion

		#region Profiles

		[OperationContract]
		[WebGet(UriTemplate = "profiles/free", ResponseFormat = WebMessageFormat.Json)]
		IQueryable<ProfileInfoContract> GetFreeReferrals();

		[OperationContract]
		[WebGet(UriTemplate = "profileinfo?id={id}", ResponseFormat = WebMessageFormat.Json)]
		ProfileInfoContract GetProfileInfoById(string id);

		[OperationContract]
		[WebGet(UriTemplate = "profiles/infobyname?name={name}", ResponseFormat = WebMessageFormat.Json)]
		ProfileInfoContract GetProfileInfoByName(string name);

		[OperationContract]
		[WebGet(UriTemplate = "profiles/get?uid={uid}&current_uid={current_uid}", ResponseFormat = WebMessageFormat.Json)]
		Stream GetCustomerId(string uid, string current_uid);

		[OperationContract]
		[WebGet(UriTemplate = "profiles/getdetailsresume?uid={uid}", ResponseFormat = WebMessageFormat.Json)]
		ProfileDetailsResumeContract GetProfileDetailsResume(Guid uid);

		[OperationContract]
		[WebInvoke(Method = "PUT", UriTemplate = "profiles/edit", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
		void EditProfile(ProfileEditContract model);

		[OperationContract]
		[WebGet(UriTemplate = "profiles/getedit?uid={uid}", ResponseFormat = WebMessageFormat.Json)]
		ProfileEditContract GetEditProfile(Guid uid);

		#endregion

		#region Users

		[OperationContract]
		[WebGet(UriTemplate = "details?uid={uid}&current={current}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
		UserDetailsContract DetailsUser(string uid, string current);

		[OperationContract]
		[WebGet(UriTemplate = "users/edit?uid={uid}&nickname={nickname}&lastDateModified={lastDateModified}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
		void EditUser(string uid, string nickname, string lastDateModified);

		[OperationContract]
		[WebGet(UriTemplate = "users/gets", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
		IQueryable<UserContract> GetUsers();

		[OperationContract]
		[WebGet(UriTemplate = "users/get?uid={uid}", ResponseFormat = WebMessageFormat.Json)]
		UserContract GetUserById(Guid uid);

		[OperationContract]
		[WebGet(UriTemplate = "userbyname?name={name}", ResponseFormat = WebMessageFormat.Json)]
		User GetUserByName(string name);

		[OperationContract]
		[WebGet(UriTemplate = "finduser?name={name}", ResponseFormat = WebMessageFormat.Json)]
		IQueryable<UserLinkContract> FindUser(string name);

		#endregion

		#region User providers

		[OperationContract]
		[WebGet(UriTemplate = "users/create_provider?providerName={providerName}&providerPageId={providerPageId}&username={username}", ResponseFormat = WebMessageFormat.Json)]
		string CreateOrUpdateAccount(string providerName, string providerPageId, string username);

		[OperationContract]
		[WebGet(UriTemplate = "users/login_provider?providerName={providerName}&providerPageId={providerPageId}", ResponseFormat = WebMessageFormat.Json)]
		LoginContract LoginExternal(string providerName, string providerPageId);

		[OperationContract]
		[WebGet(UriTemplate = "users/providers?uid={uid}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
		IQueryable<UserProvider> GetUserProviders(Guid uid);

		#endregion

		#region Roles

		[OperationContract]
		[WebGet(UriTemplate = "roles/gets", ResponseFormat = WebMessageFormat.Json)]
		IQueryable<RoleListContract> GetRoles();

		#endregion
	}
}