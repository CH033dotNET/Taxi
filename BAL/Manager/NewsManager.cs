using AutoMapper;
using BAL.Interfaces;
using DAL.Interface;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Manager
{
	public class NewsManager : BaseManager, INewsManager
	{
		public NewsManager(IUnitOfWork uOW) : base(uOW)
		{

		}

		public IEnumerable<NewsDTO> GetAllNews()
		{
			var news = uOW.NewsRepo.All.ToList();
			return news.Select(x => Mapper.Map<NewsDTO>(x));
		}

		public IEnumerable<NewsDTO> GetLatestNews(int i)
		{
			var news = uOW.NewsRepo.All.OrderBy(e => e.CreatedTime).Take(i).ToList();
			return news.Select(x => Mapper.Map<NewsDTO>(x));
		}
	}
}
