using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
	public interface IFeedbackManager
	{
		FeedbackDTO AddFeedback(FeedbackDTO feedback);
		void DeleteFeedback(int feedbackId);
		FeedbackDTO GetById(int id);
		FeedbackDTO UpdateAddress(FeedbackDTO feedback);
	}
}
