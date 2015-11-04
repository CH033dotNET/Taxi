using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enum.CarEnums;
using Common.Enum.DriverEnum;
using Common.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DB
{
	public class WorkerStatus
	{
		[Key]
		public int Id { get; set; }

		public DriverWorkingStatusEnum WorkingStatus { get; set; }

		[ForeignKey("WorkerId")]
		public virtual User Worker { get; set; }
		public int WorkerId { get; set; }
	}
}
