using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enum;

namespace Model.DTO
{
	public class OrderExDTO
	{
		public int Id { get; set; }

		public string Address { get; set; }

        public OrderStatusEnum Status { get; set; }

        public User Driver { get; set; }

        public int WaitingTime { get; set; }
    }
}
