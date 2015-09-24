using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Model
{
	public class VIPClient
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("UserId")]

		public virtual User User { get; set; }
		public int UserId { get; set; }

		public DateTime SetDate { get; set; }

	}
}
