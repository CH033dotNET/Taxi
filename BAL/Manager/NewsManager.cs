using AutoMapper;
using BAL.Interfaces;
using Common.Enum;
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
			var news = uOW.NewsRepo.All.Where(e => e.Status == ArticleStatus.Active).ToList();
			return news.Select(x => Mapper.Map<NewsDTO>(x));
		}

		public IEnumerable<NewsDTO> GetLatestNews(int i)
		{
			var news = uOW.NewsRepo
								.All
								.Where(e => e.Status == ArticleStatus.Active)
								.OrderBy(e => e.CreatedTime).Take(i).ToList();

			return news.Select(x => Mapper.Map<NewsDTO>(x));
		}

		public NewsDTO GetOneArticle(int id)
		{
			var article = uOW.NewsRepo.GetByID(id);
			return Mapper.Map<NewsDTO>(article);
		}

		public bool DeleteArticle(int id)
		{
			try
			{
				var articleDb = uOW.NewsRepo.GetByID(id);
				articleDb.Status = ArticleStatus.Deleted;
				uOW.Save();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}
