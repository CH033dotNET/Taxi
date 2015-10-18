using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainSaite.Models
{
	public class AutoParkViewModel
	{
		public CarDTO Car { get; set; }
		public List<CarDTO> Cars { get; set; }
		public List<UserDTO> Drivers { get; set; }
	}
}