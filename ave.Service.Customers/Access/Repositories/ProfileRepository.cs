using ave.DTO.Customers.Profiles.Types;
using ave.Service.Customers.Data;
using ave.Service.Customers.Data.Entities;
using ave.Service.Customers.Access.Repositories.Interfaces;
using System;
using System.Linq;

namespace ave.Service.Customers.Access.Repositories
{
	public class ProfileRepository : IProfileRepository
	{
		private readonly EFServiceRepository_Helpers.IUnitOfWork unitOfWork = null;

		/// <summary>
		/// Конструктор класса по умочанию
		/// </summary>
		/// <param name="context"></param>
		public ProfileRepository(DatabaseContext context)
		{
			unitOfWork = new EFServiceRepository_Helpers.UnitOfWork(context);
		}

		/// <summary>
		/// Создает новый профиль
		/// </summary>
		/// <param name="name">ФИО пользователя</param>
		/// <param name="gender">Пол</param>
		/// <param name="birthdate">День рождения</param>
		public void Create(string name, GenderTypes gender, DateTime birthdate)
		{
			Profile profileEntity = new Profile();
			profileEntity.profileId = Guid.NewGuid();
			profileEntity.name = name;
			profileEntity.nameVisibility = (int)FieldVisibilityTypes.Everyone;
			profileEntity.gender = (int)gender;
			profileEntity.genderVisibility = (int)FieldVisibilityTypes.Everyone;
			profileEntity.birthdate = DateTime.Now;
			profileEntity.birthdateVisibility = (int)FieldVisibilityTypes.Everyone;
			profileEntity.avatar = "/Resources/Images/Avatars/noavatar.png";
			profileEntity.status = (int)ProfileTypes.Active;

			unitOfWork.RepositoryFor<Profile>().Add(profileEntity);
			unitOfWork.Commit();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="Name"></param>
		/// <param name="NameVisibility"></param>
		/// <param name="gender"></param>
		/// <param name="birthdate"></param>
		public void Edit(Guid userId, string name, FieldVisibilityTypes nameVisibility, int gender, DateTime birthdate)
		{
			//TableProfile profileEntity = m_unitOfWork.RepositoryFor<TableProfile>().First(i => i.User.ID == userId);
			User userEntity = unitOfWork.RepositoryFor<User>().First(i => i.userId == userId);
			Profile profileEntity = unitOfWork.RepositoryFor<Profile>().First(i => i.profileId == userEntity.profile.profileId);

			profileEntity.name = name;
			profileEntity.nameVisibility = (int)nameVisibility;
			profileEntity.gender = gender;
			profileEntity.birthdate = birthdate;

			unitOfWork.RepositoryFor<Profile>().Update(profileEntity);
			unitOfWork.Commit();
		}

		/// <summary>
		/// Получает профиль пользователя по его идентификатору
		/// </summary>
		/// <param name="id">Идентификатор профиля</param>
		/// <returns>Возвращает профиль пользователя</returns>
		public Profile GetProfileById(Guid id)
		{
			return unitOfWork.RepositoryFor<Profile>().First(i => i.profileId == id);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IQueryable<Profile> GetProfiles()
		{
			return unitOfWork.RepositoryFor<Profile>().GetAll();
		}

		/// <summary>
		/// Находит все профили по имени пользователя
		/// </summary>
		/// <param name="name">ФИО пользователя</param>
		/// <returns>Возвращает коллекцию профилей</returns>
		public IQueryable<Profile> FindProfileByFullName(string name)
		{
			//return m_unitOfWork.RepositoryFor<TableProfile>().Find(i =>
			//    i.First.StartsWith(first) && i.Last.StartsWith(last));

			return unitOfWork.RepositoryFor<Profile>().Find(i => i.name.StartsWith(name));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="value"></param>
		public void ChangeKarma(Guid id, int value)
		{
			Profile profile = unitOfWork.RepositoryFor<Profile>().First(i => i.profileId == id);
			profile.karma = value;

			unitOfWork.RepositoryFor<Profile>().Update(profile);
			unitOfWork.Commit();
		}
	}
}