using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
namespace BAL.Manager
{
	public interface IOrderManager
	{
		void DeleteOrderByID(int? id);
		OrderDTO EditOrder(Model.DTO.OrderDTO order);
		OrderDTO GetNotStartOrderByDriver(int? id);
        OrderDTO GetOrderByOrderID(int? id);// GetQueryable
		IEnumerable<OrderDTO> GetOrders();
        IQueryable<Model.DB.Order> GetQueryableOrders();
        IEnumerable<OrderDTO> GetDriverOrders();
		IEnumerable<OrderDTO> GetOrdersByPersonId(int? id);
		IEnumerable<OrderDTO> GetOrdersByUserId(int id);
		OrderDTO GetStartedOrderByDriver(int? id);
		OrderDTO InsertOrder(OrderDTO order);
		IQueryable<IGrouping<int, OrderDTO>> GetTop10();
	}
}
