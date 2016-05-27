﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
	public class Feedback
	{
		public int Id { get; set; }

		public string Comment { get; set; }

		public int? Rating { get; set; }

		public int UserId { get; set; }
	}
}
