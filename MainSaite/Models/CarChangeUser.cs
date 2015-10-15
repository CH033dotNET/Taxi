using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainSaite.Models
{
	public class CarChangeUser
	{
		public CarDTO Car { get; set; }
		public List<UserDTO> Drivers { get; set; }

	}
}