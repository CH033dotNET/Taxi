﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainSaite.Models
{
	public class SignalRUser
	{
		public string ConnectionId { get; set; }
		public int UserId { get; set; }
		public int RoleId { get; set; }
		public string Group { get; set; } 
	}
}