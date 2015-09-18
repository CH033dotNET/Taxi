using Common.Enum;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
	public class RegistrationModel: UserDTO
	{
		[DataType(DataType.Password)]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }

		public RegistrationModel()
		{
			RoleId = (int)AvailableRoles.Client;
		}

	}
}
