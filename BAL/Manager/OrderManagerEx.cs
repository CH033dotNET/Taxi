using BAL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;
using Model.DB;
using Model.DTO;
using AutoMapper;
using Common.Enum;
using Model;
using BAL.Tools;
using System.Data.Entity.Core.Objects;

namespace BAL.Interfaces
{
	public class OrderManagerEx : BaseManager, IOrderManagerEx
	{
		public OrderManagerEx(IUnitOfWork uOW) : base(uOW)
		{

		}

		public OrderExDTO AddOrder(OrderExDTO order)
		{
			var newOrder = Mapper.Map<OrderEx>(order);
            newOrder.OrderTime = DateTime.Now;
			newOrder.Status = OrderStatusEnum.NotApproved;
			uOW.OrderExRepo.Insert(newOrder);
			uOW.Save();
			return Mapper.Map<OrderExDTO>(newOrder);
		}

		public void UpdateOrder(OrderExDTO order)
		{
			var result = uOW.OrderExRepo.All.Include(o => o.AddressFrom).SingleOrDefault(o=>o.Id == order.Id);

			if (result != null)
			{
				result.AddressFrom.Address = order.AddressFrom.Address;
				result.Status = order.Status;

				uOW.Save();
			}
		}

		public OrderExDTO GetById(int id) {
			var order = uOW.OrderExRepo.All.Include(o => o.AddressFrom).
				Include(o=>o.AddressesTo).
				Include(o=>o.AdditionallyRequirements).
				Where(o => o.Id == id).FirstOrDefault();

			return Mapper.Map<OrderExDTO>(order);
		}

		public bool ApproveOrder(int id)
		{
			var order = uOW.OrderExRepo.All.Where(o => o.Id == id).FirstOrDefault();
			if (order != null)
			{
				order.Status = OrderStatusEnum.Approved;
				uOW.OrderExRepo.Update(order);
				uOW.Save();
				return true;
			}
			return false;
		}

		public bool DenyOrder(int id)
		{
			var order = uOW.OrderExRepo.All.Where(o => o.Id == id).FirstOrDefault();
			if (order != null)
			{
				order.Status = OrderStatusEnum.Denied;
				uOW.OrderExRepo.Update(order);
				uOW.Save();
				return true;
			}
			return false;
		}

		public bool TakeOrder(int id, int DriverId)
		{
			var order = uOW.OrderExRepo.All.Where(o => o.Id == id).FirstOrDefault();
			if (order != null)
			{
				order.Status = OrderStatusEnum.Confirmed;

				var driver = uOW.UserRepo.All.Where(u => u.Id == DriverId).FirstOrDefault();
				order.Driver = driver;
				order.Car = uOW.CarRepo.All.Where(c => c.UserId == driver.Id && c.isMain).FirstOrDefault();

				uOW.OrderExRepo.Update(order);
				uOW.Save();
				return true;
			}
			return false;
		}

		public bool SetWaitingTime(int id, int WaitingTime)
		{
			var order = uOW.OrderExRepo.All.Where(o => o.Id == id).FirstOrDefault();
			if (order != null)
			{
				order.WaitingTime = WaitingTime;
				uOW.OrderExRepo.Update(order);
				uOW.Save();
				return true;
			}
			return false;
		}

		public IEnumerable<OrderExDTO> GetNotApprovedOrders()
		{
			var orders = uOW.OrderExRepo.All.Include(o => o.AddressFrom)
				.Where(o => o.Status == OrderStatusEnum.NotApproved)
				.ToList();
			return Mapper.Map<List<OrderExDTO>>(orders);
		}

		public IEnumerable<OrderExDTO> GetApprovedOrders()
		{
			var orders = uOW.OrderExRepo.All.Include(o => o.AddressFrom)
				.Where(o => o.Status == OrderStatusEnum.Approved)
				.ToList();
			return Mapper.Map<List<OrderExDTO>>(orders);
		}

		public IEnumerable<OrderExDTO> GetOrdersByDriver(UserDTO Driver)
		{
			var driver = Mapper.Map<User>(Driver);
			var orders = uOW.OrderExRepo.All.Include(o => o.AddressFrom)
				.Where(o => o.Driver.Id == driver.Id)
				.ToList();
			return Mapper.Map<List<OrderExDTO>>(orders);
		}

		public IEnumerable<OrderExDTO> GetOrdersByUserId(int id)
		{
			var orders = uOW.OrderExRepo.All.Include(o => o.AddressFrom)
				.Where(o => o.UserId == id && o.Status == OrderStatusEnum.Confirmed)
				.ToList();
			return Mapper.Map<List<OrderExDTO>>(orders);
		}

		public IList<OrderExDTO> GetDriversTodayOrders(UserDTO Driver) {
			var driver = Mapper.Map<User>(Driver);
			var orders = uOW.OrderExRepo.All
				.Where(o => o.Driver.Id == driver.Id 
				&& o.OrderTime.Day == DateTime.Now.Day
				&& o.OrderTime.Month == DateTime.Now.Month
				&& o.OrderTime.Year == DateTime.Now.Year)
				.ToList();
			return Mapper.Map<List<OrderExDTO>>(orders);
		}

		public void SetDriverFeedback(int orderId, int feedbackId)
		{
			var order = uOW.OrderExRepo.GetByID(orderId);
			order.DriverFeedbackId = feedbackId;
			uOW.OrderExRepo.Update(order);
			uOW.Save();
		}

		public void SetClientFeedback(int orderId, int feedbackId)
		{
			var order = uOW.OrderExRepo.GetByID(orderId);
			order.ClientFeedbackId = feedbackId;
			uOW.OrderExRepo.Update(order);
			uOW.Save();
		}

		public IEnumerable<OrderExDTO> GetLastDeniedOrders()
		{
			var orders = uOW.OrderExRepo.All.Include(o => o.AddressFrom)
				.Where(o => o.Status == OrderStatusEnum.Denied)
				.TakeLast(50)
				.ToList();
			return Mapper.Map<List<OrderExDTO>>(orders);
		}

		public IEnumerable<OrderExDTO> GetInProgressOrders()
		{
			var orders = uOW.OrderExRepo.All.Include(o => o.AddressFrom)
				.Where(o => o.Status == OrderStatusEnum.Confirmed)
				.ToList();
			return Mapper.Map<List<OrderExDTO>>(orders);
		}
	}
}
