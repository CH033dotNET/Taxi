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
		public User Sender { get; set; }
		public User Receiver { get; set; }
		public DateTime SendTime { get; set; }
	}
}
