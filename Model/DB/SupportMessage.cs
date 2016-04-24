using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
	public class SupportMessage
	{
		[Key]
		public int Id { get; set; }
		public string Message { get; set; }
		public virtual User Sender { get; set; }
		public int SenderId { get; set; }
		public virtual User Receiver { get; set; }
		public int ReceiverId { get; set; }
		public DateTime SendTime { get; set; }
	}
}
