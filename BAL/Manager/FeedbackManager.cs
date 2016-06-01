using BAL.Interfaces;
using Model.DTO;
using DAL.Interface;
using AutoMapper;
using Model.DB;
using System.Linq;

namespace BAL.Manager
{
	public class FeedbackManager : BaseManager, IFeedbackManager
	{
		public FeedbackManager(IUnitOfWork uOW)
			: base(uOW)
		{

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

		public void SetUserId(int feedbackId, int? UserId)
		{
			var feedback = uOW.FeedbackRepo.GetByID(feedbackId);
			feedback.UserId = UserId != null ? (int)UserId : 0;
			uOW.FeedbackRepo.Update(feedback);
			uOW.Save();
		}
	}
}
