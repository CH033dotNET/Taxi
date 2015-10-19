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
		[Required]
		[DisplayFormat(DataFormatString = "{hh-mm}", ApplyFormatInEditMode = true)]
		public DateTime RunTime { get; set; }
		[Required]
		public float LatitudeComeOut { get; set; }
		[Required]
		public float LongitudeComeOut { get; set; }
		[Required]
		public float LongitudeAccuracy { get; set; }
		[Required]
		public float LatitudeComeIn { get; set; }
		[Required]
		public float LongitudeComeIn { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }
		public int UserId { get; set; }
	}
}
