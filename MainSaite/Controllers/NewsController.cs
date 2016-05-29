using BAL.Interfaces;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class NewsController : BaseController
    {
		//
		// GET: /News/

		private INewsManager NewsManager;

		public NewsController(INewsManager newsManager)
		{
			NewsManager = newsManager;
		}
		public ActionResult Index()
        {
            return View(NewsManager.GetAllNews());
        }

		public bool DeleteArticle(int id)
		{
			return NewsManager.DeleteArticle(id);
		}

		public ActionResult Edit(int id)
		{
			if (id == -1)
			{
				var emptyArticle = new NewsDTO();
				emptyArticle.Id = -1;
				return View(emptyArticle);
			} else
			{
				return View(NewsManager.GetOneArticle(id));
			}
		}

		public bool SaveArticle(int id, string title, string article)
		{
			var newArticle = new NewsDTO();
			newArticle.Id = id;
			newArticle.Title = title;
			newArticle.Article = article;
			return NewsManager.SaveArticle(newArticle);
		}
	}
}
