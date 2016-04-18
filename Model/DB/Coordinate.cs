﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
	[Table("DistrictCoordinates")]
	public class Coordinate
	{
		public int Id { get; set; }

		public double Latitude { get; set; }

		public double Longitide { get; set; }
	}
}
