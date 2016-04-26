﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class CoordinateDTO
	{
		public int Id { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public int Index { get; set; }

		public int DistrictId { get; set; }
	}
}
