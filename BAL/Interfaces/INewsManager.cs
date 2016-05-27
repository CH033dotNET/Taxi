using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
	public interface INewsManager
	{
		IEnumerable<NewsDTO> GetAllNews();
		IEnumerable<NewsDTO> GetLatestNews(int i);
	}
}
