using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class WorkshiftHistoryDTO
	{
		[Key]
		public int Id { get; set; }
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh-mm-ss}", ApplyFormatInEditMode = true)]
		public Nullable<DateTime> WorkStarted { get; set; }
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh-mm-ss}", ApplyFormatInEditMode = true)]
		public Nullable<DateTime> WorkEnded { get; set; }
		[ForeignKey("DriverId")]
		public virtual User Driver { get; set; }
		public int DriverId { get; set; }
	}
}
