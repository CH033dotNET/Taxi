﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
	public class OrderEx
	{
		[Key]
		public int Id { get; set; }

		public string Address { get; set; }
	}
}