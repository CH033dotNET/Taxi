using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class UserDTO
    {
		
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

		public int RoleId { get; set; }
        public Role Role { get; set; }

		public UserDTO()
		{
			Role role = new Role() { Id = (int)AvailableRoles.Client, Name = AvailableRoles.Client.ToString(), Description = "" };
			Role = role;
			RoleId = role.Id;
		}
    }
}
