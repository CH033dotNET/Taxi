using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class LoginModel
	{
		[Required]
		[MinLength(4, ErrorMessage = "Минимальная длинна - 4 символа")]
		public string UserName { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[MinLength(6, ErrorMessage = "Минимальная длинна - 6 символа")]
		public string Password { get; set; }
	}
}
