﻿using System;
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
	public class OrderManager : BaseManager, IOrderManager
	{
	
		public OrderManager(IUnitOfWork uOW)
			:base(uOW)
		{

		}
		public OrderDTO InsertOrder(OrderDTO order)
		{
			var temp = Mapper.Map<Order>(order);
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
			var newOrder = uOW.OrderRepo.Get(s => s.PersonId == order.PersonId).Last();
			if (newOrder == null)
			{
				return null;
			}

			uOW.OrderRepo.SetStateModified(newOrder);
			newOrder.DropPlace = order.DropPlace;
			newOrder.PeekPlace = order.PeekPlace;
			newOrder.LatitudePeekPlace = order.LatitudePeekPlace;
			newOrder.LatitudeDropPlace = order.LatitudeDropPlace;
			newOrder.Accuracy = order.Accuracy;
			newOrder.LongitudePeekPlace = order.LongitudePeekPlace;
			newOrder.LongitudeDropPlace = order.LongitudeDropPlace;
			newOrder.RunTime = order.RunTime;
			newOrder.OrderTime = order.OrderTime;
			newOrder.PersonId = order.PersonId;
			newOrder.StartWork = order.StartWork;
			newOrder.EndWork = order.EndWork;
			newOrder.WaitingTime = order.WaitingTime;
			uOW.Save();
			return Mapper.Map<OrderDTO>(newOrder);
		}

		public IEnumerable<OrderDTO> GetOrders()
		{
			var orderList = uOW.OrderRepo.Get().Select(s => Mapper.Map<OrderDTO>(s));
			return orderList;
		}

		public IEnumerable<OrderDTO> GetOrdersByPersonId(int? id)
		{
			if (id == 0)
			{
				return null;
			}
			var order = uOW.OrderRepo.Get().Where(s => s.PersonId == id);
			if (order != null)
			{
				return Mapper.Map<IEnumerable<OrderDTO>>(order);
			}
			return null;
		}

        public IEnumerable<OrderDTO> GetOrdersByUserId(int id)
        {
            int pid = new PersonManager(uOW).GetPersonByUserId(id).Id;
            return GetOrdersByPersonId(pid);
        }

		public OrderDTO GetNotStartOrderByDriver(int? id)
		{
			if (id == 0)
			{
				return null;
			}
			var order = uOW.OrderRepo.Get().Where(s => s.DriverId == id && s.StartWork == null).FirstOrDefault();
			if (order != null)
			{
				return Mapper.Map<OrderDTO>(order);
			}
			return null;

		}
		public OrderDTO GetStartedOrderByDriver(int? id)
		{
			if (id == 0)
			{
				return null;
			}
			var order = uOW.OrderRepo.Get().Where(s => s.DriverId == id && s.StartWork != null && s.EndWork == null).FirstOrDefault();
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
