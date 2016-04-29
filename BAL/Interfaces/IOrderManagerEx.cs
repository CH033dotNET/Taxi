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

		bool ApproveOrder(int id);

		bool DenyOrder(int id);

        bool TakeOrder(int id);

        IEnumerable<OrderExDTO> GetNotApprovedOrders();

        IEnumerable<OrderExDTO> GetApprovedOrders();
    }
}
