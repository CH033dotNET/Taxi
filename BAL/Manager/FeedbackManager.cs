using BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DTO;
using DAL.Interface;
using AutoMapper;
using Model.DB;

namespace BAL.Manager
{
	public class FeedbackManager : BaseManager, IFeedbackManager
	{
		public FeedbackManager(IUnitOfWork uOW)
			: base(uOW)
		{
			Mapper.CreateMap<FeedbackDTO, Feedback>();
			Mapper.CreateMap<Feedback, FeedbackDTO>();

		}
		public FeedbackDTO AddFeedback(FeedbackDTO feedback)
		{
			var newFeedback = Mapper.Map<Feedback>(feedback);
			uOW.FeedbackRepo.Insert(newFeedback);
			uOW.Save();
			return Mapper.Map<FeedbackDTO>(newFeedback);
		}

		public void DeleteFeedback(int feedbackId)
		{
			uOW.FeedbackRepo.Delete(feedbackId);
			uOW.Save();
		}

		public FeedbackDTO GetById(int id)
		{
			return Mapper.Map<FeedbackDTO>(uOW.FeedbackRepo.GetByID(id));
		}

		public FeedbackDTO UpdateFeedback(FeedbackDTO feedback)
		{
			var update = Mapper.Map<Feedback>(feedback);
			uOW.FeedbackRepo.Update(update);
			uOW.Save();
			return Mapper.Map<FeedbackDTO>(update);
		}
	}
}
