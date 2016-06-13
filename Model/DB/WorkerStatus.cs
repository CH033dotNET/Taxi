using Common.Enum.DriverEnum;
using System;
using System.ComponentModel.DataAnnotations;
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

		public DateTime? BlockTime { get; set; }

		public string BlockMessage { get; set; }
	}
}
