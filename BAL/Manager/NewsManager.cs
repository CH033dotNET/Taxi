using AutoMapper;
using BAL.Interfaces;
using Common.Enum;
using DAL.Interface;
using Model.DB;
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
			var news = uOW.NewsRepo.All.Where(e => e.Status == ArticleStatus.Active).OrderByDescending(e => e.CreatedTime).ToList();
			return news.Select(x => Mapper.Map<NewsDTO>(x));
		}

		public IEnumerable<NewsDTO> GetLatestNews(int i)
		{
			var news = uOW.NewsRepo
								.All
								.Where(e => e.Status == ArticleStatus.Active)
								.OrderByDescending(e => e.CreatedTime).Take(i).ToList();

			return news.Select(x => Mapper.Map<NewsDTO>(x));
		}

		public NewsDTO GetOneArticle(int id)
		{
			var article = uOW.NewsRepo.GetByID(id);
			return Mapper.Map<NewsDTO>(article);
		}

		public bool SaveArticle(NewsDTO article)
		{
			try
			{
				if (article.Id == -1)
				{
					var newArticle = Mapper.Map<News>(article);
					newArticle.CreatedTime = DateTime.UtcNow;
					uOW.NewsRepo.Insert(newArticle);
				}
				else
				{
					var articleDb = uOW.NewsRepo.GetByID(article.Id);
					articleDb.Title = article.Title;
					articleDb.Article = article.Article;
				}

				uOW.Save();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
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
