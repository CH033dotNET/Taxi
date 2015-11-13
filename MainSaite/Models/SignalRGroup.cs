using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainSaite.Models
{
	public class SignalRGroup
	{
		public string GroupName { get; set; }
		public virtual ICollection<SignalRUser> Users { get; set; }
	}
}