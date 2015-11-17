using Model.DTO;
using System;
namespace BAL.Manager
{
	public interface IOrderManager
	{
		void DeleteOrderByID(int? id);
		Model.DTO.OrderDTO EditOrder(Model.DTO.OrderDTO order);
		Model.DTO.OrderDTO GetNotStartOrderByDriver(int? id);
        Model.DTO.OrderDTO GetOrderByOrderID(int? id);// GetQueryable
		System.Collections.Generic.IEnumerable<Model.DTO.OrderDTO> GetOrders();
        System.Linq.IQueryable<Model.DB.Order> GetQueryableOrders();
        System.Collections.Generic.IEnumerable<Model.DTO.OrderDTO> GetDriverOrders();
		System.Collections.Generic.IEnumerable<Model.DTO.OrderDTO> GetOrdersByPersonId(int? id);
		System.Collections.Generic.IEnumerable<Model.DTO.OrderDTO> GetOrdersByUserId(int id);
		Model.DTO.OrderDTO GetStartedOrderByDriver(int? id);
		Model.DTO.OrderDTO InsertOrder(Model.DTO.OrderDTO order);
	}
}
