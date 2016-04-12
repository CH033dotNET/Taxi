using System;
using System.Linq;
using Model.DTO;
using System.Collections.Generic;

namespace BAL.Manager
{
	public interface IUserManager
	{
		void ChangeUserParameters(UserDTO user);
		void DeleteUser(int userId);
		void deleteVIPById(int id);
		Model.Role GerRoleForUser(UserDTO user);
		UserDTO GetByEmail(string email, string password);
		UserDTO GetById(int id);
		UserDTO GetByUserName(string login, string password);
		List<UserDTO> GetDrivers();
		IQueryable<Model.User> GetQueryableDrivers();
		List<UserDTO> GetDriversExceptCurrent(int id);
		Pager<UserDTO> GetUserPage(string searchString, int page, int pageSize, int roleId);
		IEnumerable<UserDTO> GetUsers();
		IQueryable<VIPClientDTO> GetVIPClients();
		bool IfEmailExists(string email);
		bool IfUserNameExists(string userName);
		UserDTO InsertUser(UserDTO user);
		bool IsAdministratorById(int id);
		bool IsUserNameCorrect(string name);
		void SetVIPStatus(int UserId);
		void UpdatePassword(string login, string password);
		UserDTO UpdateUser(UserDTO user);
		bool UserValidation(UserDTO user, List<string> msgs);
	}
}
