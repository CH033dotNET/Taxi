﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class ClientOrderedDTO
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public string WaitingTime { get; set; }
	}
}

