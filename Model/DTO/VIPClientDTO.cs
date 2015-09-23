using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class VIPClientDTO
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public DateTime SetDate { get; set; }
	}
}
