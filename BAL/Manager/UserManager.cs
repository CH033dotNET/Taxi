﻿using System;
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
using System.Data.Entity;


namespace BAL.Manager
{
	public class UserManager : BaseManager, IUserManager
	{
		public UserManager(IUnitOfWork uOW)
			:base(uOW)
		{
			
		}

		public void SetClientBonus(int userId, double bonus, double paidByBonus)
		{
			var user = uOW.UserRepo.All.Where(u => u.Id == userId).FirstOrDefault();
			if (user != null)
			{
				user.Bonus -= paidByBonus;
				user.Bonus += bonus;
				uOW.Save();
			}

		}

		public PagerDTO<UserDTO> GetUserPage(string searchString, int page, int pageSize, int roleId)
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

			var model = new PagerDTO<UserDTO>
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
			var item = uOW.UserRepo.All
				.Where(s => (s.UserName.ToUpper() == login.ToUpper() && s.Password == password)).Include("Role")
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

		public UserDTO AddUser(RegistrationModelDTO user)
		{
			return InsertUser(Mapper.Map<UserDTO>(user));
		}

		public UserDTO InsertUser(UserDTO user)
		{
			//var temp = Mapper.Map<User>(user);

			user.UserName = user.UserName.Trim();
			user.Password = user.Password.Trim();
	        user.Email = user.Email.Trim();

			var role = GerRoleForUser(user);

			user.Role = role;
			user.RoleId = role.Id;
			user.RegistrationDate = DateTime.Now;
			var temp = Mapper.Map<User>(user);

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
            temp.Password = user.Password;
            //temp.Lang = user.Lang;
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


		///SetVIPStatus methodes
		public IQueryable<VIPClientDTO> GetVIPClients()
		{
			var VipClients =
					from U in uOW.UserRepo.All.Where(x => x.RoleId == 3)
					join V in uOW.VIPClientRepo.All
						on U.Id equals V.UserId into joined
					from V in joined.DefaultIfEmpty()
					select new VIPClientDTO
					{
						Id = V != null ? V.Id : 0,
						SetDate = V != null ? V.SetDate : DateTime.MaxValue,
						UserId = U.Id,
						UserName = U.UserName
					};

			return VipClients.OrderByDescending(x => x.Id);
		}

		public void SetVIPStatus(int userId)
		{
			uOW.VIPClientRepo.Insert(new VIPClient { UserId = userId, SetDate = System.DateTime.Today });
			uOW.Save();
		}

		public void deleteVIPById(int id)
		{
			uOW.VIPClientRepo.Delete(uOW.VIPClientRepo.GetByID(id));
			uOW.Save();
		}



		public bool IsUserNameCorrect(string name)
		{
			for (int index = 0; index < name.Length; index++)
				if (!Char.IsLetterOrDigit(name[index]))
					return false;
			return true;
		}


		//
		/// end of SetVIPStatu methodes

		#region OUR TEAM METHODS

		public Role GerRoleForUser(UserDTO user)
		{
     		var a = uOW.RoleRepo.All.Where(x => x.Name == user.Role.Name).First();

			return a;
		}

		public Role GerRoleForUser(RegistrationModelDTO user)
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

		public List<UserDTO> GetDrivers()
		{

			List<User> drivers = uOW.UserRepo.All.Where(x => x.RoleId == (int)AvailableRoles.Driver || x.RoleId == (int)AvailableRoles.FreeDriver).ToList();
			List<UserDTO> driversDTO = new List<UserDTO>();

			foreach (var i in drivers)
			{
				driversDTO.Add(Mapper.Map<UserDTO>(i));
			}


			return driversDTO;
		}


        public IQueryable<User> GetQueryableDrivers()
        {
            return uOW.UserRepo.All.Where(x => x.RoleId == (int)AvailableRoles.Driver);
        }


		public List<UserDTO> GetDriversExceptCurrent(int id)
		{
			List<UserDTO> driversList = uOW.UserRepo.Get(x => x.RoleId == (int)AvailableRoles.Driver & x.Id != id).Select(s => Mapper.Map<UserDTO>(s)).ToList();
			return driversList;
		}

		public IEnumerable<DriverWithOrdersDTO> GetDriversWithOrders()
		{
			var drivers = Mapper.Map<List<UserDTO>>(uOW.UserRepo.All.Where(u => u.RoleId == (int)AvailableRoles.Driver));
			var driversWithOrders = new List<DriverWithOrdersDTO>();
			foreach (var driver in drivers)
			{
				var driverWithOrders = new DriverWithOrdersDTO();
				driverWithOrders.Driver = driver;
				driverWithOrders.OrdersCount = uOW.OrderExRepo.All.Where(o => o.DriverId == driver.Id).Count();
				var person = Mapper.Map<PersonDTO>(uOW.PersonRepo.All.Where(p => p.UserId == driver.Id).FirstOrDefault());
				if (person != null)
				{
					if (person.ImageName != null)
						driverWithOrders.Image = person.ImageName;
					else
						driverWithOrders.Image = "item_0_profile.jpg";
					driverWithOrders.Name = person.FullName;
				}
				else
					driverWithOrders.Image = "item_0_profile.jpg";
				driversWithOrders.Add(driverWithOrders);
			}
			driversWithOrders.Sort(delegate(DriverWithOrdersDTO x, DriverWithOrdersDTO y)
			{
				return y.OrdersCount.CompareTo(x.OrdersCount);
			});
			return driversWithOrders;
		}

		public IEnumerable<DriverWithOrdersDTO> GetDriversWithOrdersLastMonth()
		{
			var drivers = Mapper.Map<List<UserDTO>>(uOW.UserRepo.All.Where(u => u.RoleId == (int)AvailableRoles.Driver));
			var driversWithOrders = new List<DriverWithOrdersDTO>();
			foreach (var driver in drivers)
			{
				var driverWithOrders = new DriverWithOrdersDTO();
				driverWithOrders.Driver = driver;
				driverWithOrders.OrdersCount = uOW.OrderExRepo.All.Where(o => o.DriverId == driver.Id
					&& o.OrderTime.Year == DateTime.Now.Year
					&& o.OrderTime.Month == DateTime.Now.Month).Count();
				var person = Mapper.Map<PersonDTO>(uOW.PersonRepo.All.Where(p => p.UserId == driver.Id).FirstOrDefault());
				if (person != null)
				{
					if (person.ImageName != null)
						driverWithOrders.Image = person.ImageName;
					else
						driverWithOrders.Image = "item_0_profile.jpg";
					driverWithOrders.Name = person.FullName;
				}
				else
					driverWithOrders.Image = "item_0_profile.jpg";
				driversWithOrders.Add(driverWithOrders);
			}
			driversWithOrders.Sort(delegate (DriverWithOrdersDTO x, DriverWithOrdersDTO y)
			{
				return y.OrdersCount.CompareTo(x.OrdersCount);
			});
			return driversWithOrders;
		}

		public List<DriverWithOrdersDTO> GetCurrentDrivers(int id)
		{
			var allDrivers = this.GetDriversWithOrders().ToList();
			var i = 0;
			for (; i < allDrivers.Count; i++)
				if (allDrivers[i].Driver.Id == id)
					break;
			var currentDriverIndex = i;
			var firstDriverIndex = i - 3;
			var lastDriverIndex = i + 3;
			while (firstDriverIndex < 0)
			{
				firstDriverIndex++;
				lastDriverIndex++;
			}
			while (lastDriverIndex >= allDrivers.Count)
			{
				firstDriverIndex--;
				lastDriverIndex--;
			}
			while (firstDriverIndex < 0)
				firstDriverIndex++;
			var currentDrivers = new List<DriverWithOrdersDTO>();
			for (i = firstDriverIndex; i <= lastDriverIndex; i++)
			{
				var newDriver = allDrivers[i];
				newDriver.Index = i + 1;
				currentDrivers.Add(newDriver);
			}
			return currentDrivers;
		}

		public List<DriverWithOrdersDTO> GetCurrentDriversLastMonth(int id)
		{
			var allDrivers = this.GetDriversWithOrdersLastMonth().ToList();
			var i = 0;
			for (; i < allDrivers.Count; i++)
				if (allDrivers[i].Driver.Id == id)
					break;
			var currentDriverIndex = i;
			var firstDriverIndex = i - 3;
			var lastDriverIndex = i + 3;
			while (firstDriverIndex < 0)
			{
				firstDriverIndex++;
				lastDriverIndex++;
			}
			while (lastDriverIndex >= allDrivers.Count)
			{
				firstDriverIndex--;
				lastDriverIndex--;
			}
			while (firstDriverIndex < 0)
				firstDriverIndex++;
			var currentDrivers = new List<DriverWithOrdersDTO>();
			for (i = firstDriverIndex; i <= lastDriverIndex; i++)
			{
				var newDriver = allDrivers[i];
				newDriver.Index = i + 1;
				currentDrivers.Add(newDriver);
			}
			return currentDrivers;
		}

		public void CalculateUserRating(int id)
		{
			var feedback = uOW.FeedbackRepo.GetByID(id);
			var user = uOW.UserRepo.GetByID(feedback.UserId);
			if (user != null)
			{
				user.Rating = uOW.FeedbackRepo.All.Where(f => f.UserId == user.Id).Average(f => f.Rating);
				uOW.UserRepo.Update(user);
				uOW.Save();
			}
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
