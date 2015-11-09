using Model.DTO;
using System;
namespace BAL.Manager
{
	public interface IOrderManager
	{
		void DeleteOrderByID(int? id);
		Model.DTO.OrderDTO EditOrder(Model.DTO.OrderDTO order);
		Model.DTO.OrderDTO GetNotStartOrderByDriver(int? id);
		Model.DTO.OrderDTO GetOrderByOrderID(int? id);
		System.Collections.Generic.IEnumerable<Model.DTO.OrderDTO> GetOrders();
        System.Collections.Generic.IEnumerable<Model.DTO.OrderDTO> GetDriverOrders();
		System.Collections.Generic.IEnumerable<Model.DTO.OrderDTO> GetOrdersByPersonId(int? id);
		System.Collections.Generic.IEnumerable<Model.DTO.OrderDTO> GetOrdersByUserId(int id);
		Model.DTO.OrderDTO GetStartedOrderByDriver(int? id);
        OrderDTO EditOrder(OrderDTO order, DateTime start, DateTime end, string price);
		Model.DTO.OrderDTO InsertOrder(Model.DTO.OrderDTO order);
	}
}
