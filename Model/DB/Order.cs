using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DTO;

namespace Model.DB
{
	public class Order
	{

		[Key]
		public int Id { get; set; }
		[Required]
		[MaxLength(120, ErrorMessage = "Максимальная длинна - 120 символов")]
		public string ComeOut { get; set; }
		[Required]
		[MaxLength(120, ErrorMessage = "Максимальная длинна - 120 символов")]
		public string ComeIn { get; set; }
		[Required]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh-mm-ss}", ApplyFormatInEditMode = true)]
		public DateTime OrderTime { get; set; }
		[MaxLength(120, ErrorMessage = "Максимальная длинна - 80 символов")]
		public string RunTime { get; set; }
		[Required]
		public float LatitudeComeOut { get; set; }
		[Required]
		public float LongitudeComeOut { get; set; }

		public float Accuracy { get; set; }
		[Required]
		public float LatitudeComeIn { get; set; }
		[Required]
		public float LongitudeComeIn { get; set; }
		
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh-mm-ss}", ApplyFormatInEditMode = true)]
		public DateTime StartWork { get; set; }
		
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh-mm-ss}", ApplyFormatInEditMode = true)]
		public DateTime EndWork { get; set; }
		public int DriverId { get; set; }
		public string WaitingTime { get; set; }

		[ForeignKey("PersonId")]
		public virtual Person Person { get; set; }
		public int PersonId { get; set; }
	}
}
