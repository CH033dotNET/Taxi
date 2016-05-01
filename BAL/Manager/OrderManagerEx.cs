using BAL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;
using Model.DB;
using Model.DTO;
using AutoMapper;
using Common.Enum;

namespace BAL.Interfaces
{
	public class OrderManagerEx : BaseManager, IOrderManagerEx
	{
		public OrderManagerEx(IUnitOfWork uOW) : base(uOW)
		{
			Mapper.CreateMap<OrderEx, OrderExDTO>();
			Mapper.CreateMap<OrderExDTO, OrderEx>();
		}

		public OrderExDTO AddOrder(OrderExDTO order)
		{
			var newOrder = Mapper.Map<OrderEx>(order);
			newOrder.Status = OrderStatusEnum.NotApproved;
			uOW.OrderExRepo.Insert(newOrder);
			uOW.Save();
			return Mapper.Map<OrderExDTO>(newOrder);
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

		public bool TakeOrder(int id)
		{
			var order = uOW.OrderExRepo.All.Where(o => o.Id == id).FirstOrDefault();
			if (order != null)
			{
				order.Status = OrderStatusEnum.Confirmed;
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
			var orders = uOW.OrderExRepo.All
				.Where(o => o.Status == OrderStatusEnum.NotApproved)
				.ToList();
			return Mapper.Map<List<OrderExDTO>>(orders);
		}

		public IEnumerable<OrderExDTO> GetApprovedOrders()
		{
			var orders = uOW.OrderExRepo.All
				.Where(o => o.Status == OrderStatusEnum.Approved)
				.ToList();
			return Mapper.Map<List<OrderExDTO>>(orders);
		}

	}
}
