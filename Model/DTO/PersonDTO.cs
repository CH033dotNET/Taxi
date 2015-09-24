using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class PersonDTO
	{
		[Key]
		public int Id { get; set; }
		
		[MaxLength(30, ErrorMessage = "Максимальная длинна - 30 символов")]
		[MinLength(3, ErrorMessage = "Минимальная длинна - 3 символов")]
		public string FirstName { get; set; }
		
		[MaxLength(30, ErrorMessage = "Максимальная длинна - 30 символов")]
		[MinLength(3, ErrorMessage = "Минимальная длинна - 3 символов")]
		public string MiddleName { get; set; }
		
		[MaxLength(30, ErrorMessage = "Максимальная длинна - 30 символов")]
		[MinLength(3, ErrorMessage = "Минимальная длинна - 3 символов")]
		public string LastName { get; set; }

		public string Phone { get; set; }

		[ForeignKey("UserId")]
		public virtual UserDTO User { get; set; }
		public int UserId { get; set; }
	}
}
