using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
	public class News
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Article { get; set; }
		public DateTime CreatedTime { get; set; }
		public ArticleStatus Status { get; set; }
	}
}
