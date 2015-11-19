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
		public Order()
		{
			this.District = null;
		}

		[Key]
		public int Id { get; set; }
		[Required]
		[MaxLength(120, ErrorMessage = "Максимальная длинна - 120 символов")]
		public string PeekPlace { get; set; }
		[Required]
		[MaxLength(120, ErrorMessage = "Максимальная длинна - 120 символов")]
		public string DropPlace { get; set; }
		[Required]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh-mm-ss}", ApplyFormatInEditMode = true)]
		public DateTime OrderTime { get; set; }
		[MaxLength(120, ErrorMessage = "Максимальная длинна - 80 символов")]
		public string RunTime { get; set; }
		[Required]
		public double LatitudeDropPlace { get; set; }
		[Required]
		public double LongitudeDropPlace { get; set; }

		public double Accuracy { get; set; }
		[Required]
		public double LatitudePeekPlace { get; set; }
		[Required]
		public double LongitudePeekPlace { get; set; }
		public int IsConfirm { get; set; }
		
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh-mm-ss}", ApplyFormatInEditMode = true)]
		public Nullable<DateTime> StartWork { get; set; }
		
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh-mm-ss}", ApplyFormatInEditMode = true)]
		public Nullable<DateTime> EndWork { get; set; }
		public int DriverId { get; set; }
		public string WaitingTime { get; set; }
		[Required]
		public decimal TotalPrice { get; set; }
		[ForeignKey("PersonId")]
		public virtual Person Person { get; set; }
		public int PersonId { get; set; }
		[ForeignKey("DistrictId")]
		public virtual District District { get; set; }
		public int? DistrictId { get; set; }
	}
}
