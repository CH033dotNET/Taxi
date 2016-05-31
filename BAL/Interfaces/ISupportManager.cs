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
		SupporterInfoDTO GetSupporter(int id = -1);
		IEnumerable<SupportMessageDTO> GetMessages(int user1Id, int user2Id);
		void SendMessage(string message, int fromUserID, int toUserID);
		List<SupporterInfoDTO> GetChatUsers();
	}
}
