using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainSaite.Models
{
	public class SignalRUserEx
	{
		public string ConnectionId { get; set; }

		public string Group { get; set; }

		public int? OrderId { get; set; }

		public int? UserId { get; set; }
	}
}