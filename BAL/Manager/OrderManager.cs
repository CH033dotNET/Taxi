using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Interface;
using Model.DB;
using Model.DTO;

namespace BAL.Manager
{
	class OrderManager : BaseManager
	{
		public OrderManager(IUnitOfWork uOW)
			: base(uOW)
		{
		}
		public OrderDTO InsertOrder(OrderDTO order)
		{
			var temp = Mapper.Map<Order>(order);
			order.ComeIn = temp.ComeIn;
			order.ComeOut = temp.ComeOut;
			order.LatitudeComeIn = temp.LatitudeComeIn;
			order.LatitudeComeOut = temp.LatitudeComeOut;
			order.LongitudeAccuracy = temp.LongitudeAccuracy;
			order.LongitudeComeIn = temp.LongitudeComeIn;
			order.LongitudeComeOut = temp.LongitudeComeOut;
			order.RunTime = temp.RunTime;
			order.OrderTime = temp.OrderTime;
			order.UserId = temp.UserId;
			uOW.OrderRepo.Insert(temp);
			uOW.Save();

			return Mapper.Map<OrderDTO>(temp);
		}

		public void DeleteOrderByID(int? id)
		{
			Order order = uOW.OrderRepo.GetByID(id);
			uOW.CarRepo.Delete(order);
			uOW.Save();
		}

		public OrderDTO EditOrder(OrderDTO order)
		{
			var newOrder = uOW.OrderRepo.Get(s => s.UserId == order.UserId).First();
			if (newOrder == null)
			{
				return null;
			}

			uOW.OrderRepo.SetStateModified(newOrder);
			newOrder.ComeIn = order.ComeIn;
			newOrder.ComeOut = order.ComeOut;
			newOrder.LatitudeComeIn = order.LatitudeComeIn;
			newOrder.LatitudeComeOut = order.LatitudeComeOut;
			newOrder.LongitudeAccuracy = order.LongitudeAccuracy;
			newOrder.LongitudeComeIn = order.LongitudeComeIn;
			newOrder.LongitudeComeOut = order.LongitudeComeOut;
			newOrder.RunTime = order.RunTime;
			newOrder.OrderTime = order.OrderTime;
			newOrder.UserId = order.UserId;

			uOW.Save();
			return Mapper.Map<OrderDTO>(newOrder);
		}

		public IEnumerable<OrderDTO> GetOrders()
		{
			var orderList = uOW.OrderRepo.Get().Select(s => Mapper.Map<OrderDTO>(s));
			return orderList;
		}

		public OrderDTO GetOrderByUserId(int? id)
		{
			if (id == 0)
			{
				return null;
			}
			var order = uOW.OrderRepo.Get().Where(s => s.UserId == id).FirstOrDefault();
			if (order != null)
			{
				return Mapper.Map<OrderDTO>(order);
			}
			return null;
		}
		public OrderDTO GetOrderByOrderID(int? id)
		{
			if (id == 0)
			{
				return null;
			}
			var order = uOW.OrderRepo.Get().Where(s => s.Id == id).FirstOrDefault();
			if (order != null)
			{
				return Mapper.Map<OrderDTO>(order);
			}
			return null;
		}
	}

}
