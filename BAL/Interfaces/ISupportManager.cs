using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
	public interface ISupportManager
	{
		SupporterInfoDTO GetSupporter();
		IEnumerable<SupportMessageDTO> GetMessages(int userId);
		void SendMessage(string message, int fromUserID, int toUserID = -1);
	}
}
