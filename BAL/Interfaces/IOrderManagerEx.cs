using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
	public interface IOrderManagerEx
	{
		OrderExDTO AddOrder(OrderExDTO order);

		OrderExDTO GetById(int id);

		bool ApproveOrder(int id);

		bool DenyOrder(int id);

		bool TakeOrder(int id, int DriverId);

		bool SetWaitingTime(int id, int WaitingTime);

		IEnumerable<OrderExDTO> GetNotApprovedOrders();

		IEnumerable<OrderExDTO> GetApprovedOrders();

		IEnumerable<OrderExDTO> GetLastDeniedOrders();

		IEnumerable<OrderExDTO> GetInProgressOrders();

		IEnumerable<OrderExDTO> GetOrdersByDriver(UserDTO Driver);

		IEnumerable<OrderExDTO> GetOrdersByUserId(int id);

		IList<OrderExDTO> GetDriversTodayOrders(UserDTO Driver);

		void UpdateOrder(OrderExDTO order);

		void SetDriverFeedback(int orderId, int feedbackId);
		void SetClientFeedback(int orderId, int feedbackId);
	}
}
