using Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class UserDTO
    {
		[Key]
        public int Id { get; set; }
		[Required]
		[MaxLength(12, ErrorMessage = "Максимальная длинна - 12 символа")]
		[MinLength(4, ErrorMessage = "Минимальная длинна - 4 символа")]
        public string UserName { get; set; }
		[Required]

		[MinLength(6, ErrorMessage = "Минимальная длинна - 6 символа")]
		[DataType(DataType.Password)]
        public string Password { get; set; }
		[Required]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

		[ForeignKey("RoleId")]
		public int RoleId { get; set; }
        public Role Role { get; set; }

		public UserDTO()
		{
			RoleId = (int)AvailableRoles.Client;
		}
    }
}
