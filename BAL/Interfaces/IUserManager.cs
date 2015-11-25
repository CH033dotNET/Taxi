using System;
using System.Linq;
namespace BAL.Manager
{
	public interface IUserManager
	{
		void ChangeUserParameters(Model.DTO.UserDTO user);
		void DeleteUser(int userId);
		void deleteVIPById(int id);
		Model.Role GerRoleForUser(Model.DTO.UserDTO user);
		Model.DTO.UserDTO GetByEmail(string email, string password);
		Model.DTO.UserDTO GetById(int id);
		Model.DTO.UserDTO GetByUserName(string login, string password);
		System.Collections.Generic.List<Model.DTO.UserDTO> GetDrivers();
        System.Linq.IQueryable<Model.User> GetQueryableDrivers();
		System.Collections.Generic.List<Model.DTO.UserDTO> GetDriversExceptCurrent(int id);
		Model.DTO.Pager<Model.DTO.UserDTO> GetUserPage(string searchString, int page, int pageSize, int roleId);
		System.Collections.Generic.IEnumerable<Model.DTO.UserDTO> GetUsers();
		IQueryable<Model.DTO.VIPClientDTO> GetVIPClients();
		bool IfEmailExists(string email);
		bool IfUserNameExists(string userName);
		Model.DTO.UserDTO InsertUser(Model.DTO.UserDTO user);
		bool IsAdministratorById(int id);
		bool IsUserNameCorrect(string name);
		void SetVIPStatus(int UserId);
		void UpdatePassword(string login, string password);
		Model.DTO.UserDTO UpdateUser(Model.DTO.UserDTO user);
		bool UserValidation(Model.DTO.UserDTO user, System.Collections.Generic.List<string> msgs);
	}
}
