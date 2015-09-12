using ave.DTO.Customers.Profiles.Types;
using ave.Service.Customers.Data.Entities;
using System;
using System.Linq;

namespace ave.Service.Customers.Access.Repositories.Interfaces
{
	public interface IProfileRepository
	{
		void Create(string name, GenderTypes gender, DateTime birthdate);
		void Edit(Guid userId, string name, FieldVisibilityTypes nameVisibility, int gender, DateTime birthdate);
		Profile GetProfileById(Guid id);
		IQueryable<Profile> GetProfiles();
		IQueryable<Profile> FindProfileByFullName(string name);
		void ChangeKarma(Guid id, int value);
	}
}