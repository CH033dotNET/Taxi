using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Interface;
using Model;
using Model.DTO;
using Common;
using Common.Enum;

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
				.Where(s => (s.UserName.ToUpper() == login.ToUpper() && s.Password == password))
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

		public UserDTO GetById(int id)
		{
			var item = uOW.UserRepo.Get().Where(s => s.Id == id)
				.FirstOrDefault();

			if (item != null)
			{
				return Mapper.Map<UserDTO>(item);
			}
			return null;
		}

		
		public bool IfUserNameExists(string userName)
		{
			var item = uOW.UserRepo.Get().Where(s => (s.UserName.ToUpper() == userName.ToUpper())).FirstOrDefault();
			if (item != null)
			{
				return true;
			}
			return false;
		}

		public bool IfEmailExists(string email)
		{
			var item = uOW.UserRepo.Get().Where(s => (s.Email.ToUpper() == email.ToUpper())).FirstOrDefault();
			if (item != null)
			{
				return true;
			}
			return false;
		}

		public UserDTO InsertUser(UserDTO user)
		{
			var temp = Mapper.Map<User>(user);
			user.UserName = temp.UserName.Trim();
			user.Password = temp.Password.Trim();
	        user.Email = temp.Email.Trim();
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
			/*if (IsAdministratorById(temp.Id))
			{
				return null;
			}*/
			uOW.UserRepo.SetStateModified(temp);
			temp.RoleId = user.RoleId;
			temp.Email = user.Email;
			temp.UserName = user.UserName;
			uOW.Save();
			return Mapper.Map<UserDTO>(temp);
		}

		// Checking validation and return some messages
		public bool UserValidation(UserDTO user, List<string> msgs)
		{
			msgs.Clear();
			bool result = true;
			if (user.UserName == null)
			{
				result = false;
				msgs.Add(Resources.Resource.EmptyUserName);

			}
			else if (user.UserName.Count() < 4)
			{
				result = false;
				msgs.Add(Resources.Resource.ShortUserName);
			}

			if (user.Password == null) 
			{
				result = false;
				msgs.Add(Resources.Resource.EmptyPassword);
			}
			else if (user.Password.Count() < 5)
			{
				result = false;
				msgs.Add(Resources.Resource.ShortPassword);

			}

			if (user.Email == null)
			{
				result = false;
				msgs.Add(Resources.Resource.EmptyEmail);
			}

			return result;
		}

		public IEnumerable<VIPClientDTO> GetVIPClients()
		{

			var listUsers = uOW.UserRepo.Get();
			var listVIPClientsDTO = new List<VIPClientDTO>();

			var listVIPClients = uOW.VIPClientRepo.Get().ToList();
			foreach (var Client in listVIPClients)
			{
				var CurrentUser = listUsers.Where(x => (x.Id == Client.UserId)).First();
				listVIPClientsDTO.Add(new VIPClientDTO { Id = Client.Id, SetDate = Client.SetDate, UserId = Client.UserId, UserName = CurrentUser.UserName });
			}

			return listVIPClientsDTO;
		}

		///SetVIPStatu methodes
		public IEnumerable<UserDTO> GetNoVIPClients()
		{
			var listVIPClients = uOW.VIPClientRepo.Get().ToList();
			var listUsers = uOW.UserRepo.Get();
			var ListUserDTO = new List<UserDTO>();


			var RigthJoin =
					from U in listUsers.Where(x => x.RoleId == 3)
					join V in listVIPClients
						on U.Id equals V.UserId into joined
					from V in joined.DefaultIfEmpty().Where(x => x == null)
					select new UserDTO
					{
						Id = U.Id,
						UserName = U.UserName
					};

			foreach (UserDTO U in RigthJoin)
			{
				ListUserDTO.Add(U);
			}

			return ListUserDTO;
		}

		public void SetVIPStatus(string UserName)
		{
			int CurrentId = uOW.UserRepo.Get(x => x.UserName == UserName).Select(d => d.Id).First();
			uOW.VIPClientRepo.Insert(new VIPClient { UserId = CurrentId, SetDate = System.DateTime.Today });
			uOW.Save();
		}

		public bool IsUserNameCorrect(string name)
		{
			for (int index = 0; index < name.Length; index++)
				if (!Char.IsLetterOrDigit(name[index]))
					return false;
						return true;
		}

		public void deleteVIPById(int id)
		{
			VIPClient a = uOW.VIPClientRepo.GetByID(id);
			uOW.VIPClientRepo.Delete(a);
			uOW.Save();
		}

		//
		/// end of SetVIPStatu methodes

		#region OUR TEAM METHODS

		public Role GerRoleForUser(UserDTO user)
		{
			var a = uOW.RoleRepo.All.Where(x => x.Name == user.Role.Name).First();

			return a;
		}

		public void ChangeUserParameters(UserDTO user)
		{
			var role = GerRoleForUser(user);
			user.Role = role;
			user.RoleId = role.Id;

			UpdateUser(user);
		}

		#endregion



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

		public IEnumerable<RoleDTO> GetRoles()
		{

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
