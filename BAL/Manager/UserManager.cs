using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Interface;
using Model;
using Model.DTO;

namespace BAL.Manager
{
	public class UserManager : BaseManager
	{
		public UserManager(IUnitOfWork uOW)
			: base(uOW)
		{
		}

		public Pager<UserDTO> GetUserPage(string searchString, int page, int pageSize, int roleId)
		{
			List<User> users = new List<User>();

			if (!String.IsNullOrEmpty(searchString))
			{
				users.AddRange(
					uOW.UserRepo.Get()
						.Where(s => (s.UserName.ToLower()).Contains(searchString.ToLower())
						            || (s.Email.ToLower()).Contains(searchString.ToLower()))
					);
			}
			else
			{
				users.AddRange(uOW.UserRepo.Get());
			}

			if (roleId > 0)
			{
				users = users.Where(s => (s.RoleId == roleId)).ToList();
			}

			var pageCount = (double) users.Count()/pageSize;

			var model = new Pager<UserDTO>
			{
				CurrentPage = page,
				PageCount = (int) Math.Ceiling(pageCount)
			};

			// if asked page is greater then avilable ones
			if (model.PageCount < page)
			{
				return model;
			}

			var skip = pageSize*(page - 1);
			var resultList = Mapper.Map<IEnumerable<UserDTO>>(users.Skip(skip).Take(pageSize)).ToList();

			model.Data = resultList;

			return model;
		}

		public IEnumerable<UserDTO> GetUsers()
		{
			var list = from user in uOW.UserRepo.Get()
				select new UserDTO
				{
					Id = user.Id,
					Email = user.Email,
					RoleId = user.RoleId,
					Role = user.Role,
					UserName = user.UserName
				};

			return list.ToList();
		}

		public UserDTO GetByUserName(string login, string password)
		{
			var item = uOW.UserRepo.Get()
				.Where(s => (s.UserName == login && s.Password == password))
				.FirstOrDefault();

			if (item != null)
			{
				return Mapper.Map<UserDTO>(item);
			}
			return null;
		}

		public void UpdatePassword(string login, string password)
		{
			var id = uOW.UserRepo.Get().Where(s => s.UserName == login).FirstOrDefault().Id;
			var item = uOW.UserRepo.GetByID(id);
			if (item != null)
			{
				item.Password = password;
				uOW.UserRepo.Update(item);
				uOW.Save();
			}
		}

		public UserDTO GetByEmail(string email, string password)
		{
			var item = uOW.UserRepo.Get()
				.Where(s => (s.Email == email && s.Password == password))
				.FirstOrDefault();

			if (item != null)
			{
				return Mapper.Map<UserDTO>(item);
			}
			return null;
		}

		public bool IfUserNameExists(string userName)
		{
			var item = uOW.UserRepo.Get().Where(s => (s.UserName == userName)).FirstOrDefault();
			if (item != null)
			{
				return true;
			}
			return false;
		}

		public bool IfEmailExists(string email)
		{
			var item = uOW.UserRepo.Get().Where(s => (s.Email == email)).FirstOrDefault();
			if (item != null)
			{
				return true;
			}
			return false;
		}

		public UserDTO InsertUser(UserDTO user)
		{
			var temp = Mapper.Map<User>(user);
			temp.UserName.Trim();
			temp.Password.Trim();
			temp.Email.Trim();

			uOW.UserRepo.Insert(temp);
			// TODO:
			//uOW.UserInfoRepo.Insert(new UserInfo { UserId = user.Id });
			uOW.Save();
			return Mapper.Map<UserDTO>(temp);
		}

		public void DeleteUser(int userId)
		{
			if (!IsAdministratorById(userId))
			{
				uOW.UserRepo.Delete(uOW.UserRepo.GetByID(userId));
				// TODO:
				//uOW.UserInfoRepo.Delete(uOW.UserInfoRepo.GetByID(userId));
				uOW.Save();
			}
			return;
		}

		public UserDTO UpdateUser(UserDTO user)
		{
			var temp = uOW.UserRepo.Get(u => u.Id == user.Id).First();
			if (temp == null)
			{
				return null;
			}
			if (IsAdministratorById(temp.Id))
			{
				return null;
			}
			uOW.UserRepo.SetStateModified(temp);
			temp.RoleId = user.RoleId;
			temp.Email = user.Email;
			uOW.Save();
			return Mapper.Map<UserDTO>(temp);
		}

		// TODO:
		/*
		public UserInfoDTO GetInfoById(int id) {
			return Mapper.Map<UserInfoDTO>(uOW.UserInfoRepo.GetByID(id));
		}


		public UserInfoDTO GetInfoByUserName(string userName) {
			var id = uOW.UserRepo.Get().Where(s => s.UserName == userName).FirstOrDefault().Id;
			return this.GetInfoById(id);
		}

		public UserInfoDTO UpdateUserInfo(UserInfoDTO userInfo) {
			var temp = Mapper.Map<UserInfo>(userInfo);
			uOW.UserInfoRepo.Update(temp);
			uOW.Save();
			return Mapper.Map<UserInfoDTO>(temp);
		}
		
		public IEnumerable<UserInfoDTO> GetUsersInfo() {
			var result = uOW.UserInfoRepo.Get(s => s.User.Role.Name != RoleNames.Administrator);
			return Mapper.Map<IEnumerable<UserInfoDTO>>(result);
		}

		public IEnumerable<RoleDTO> GetRoles() {
			return Mapper.Map<IEnumerable<RoleDTO>>(uOW.RoleRepo.Get());
		}
		*/

		// TODO:
		public bool IsAdministratorById(int id)
		{
			throw new NotImplementedException();
			/*var item = uOW.UserRepo.Get().Where(s => s.Id == id).FirstOrDefault();
			if (item.Role.Name == RoleNames.Administrator) {
				return true;
			}
			return false;*/
		}
	}
}
