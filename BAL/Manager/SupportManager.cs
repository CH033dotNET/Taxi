using AutoMapper;
using BAL.Interfaces;
using Common.Enum;
using DAL.Interface;
using Model;
using Model.DB;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Manager
{
	public class SupportManager : BaseManager, ISupportManager
	{
		public SupportManager(IUnitOfWork uOW) : base(uOW) { }

		public IEnumerable<SupportMessageDTO> GetMessages(int userId)
		{
			var messages = uOW.SupportRepo
							.Get(e => e.Receiver.Id == userId || e.Sender.Id == userId)
							.Select(s => Mapper.Map<SupportMessageDTO>(s));

			return messages;
		}

		public void SendMessage(string message, int fromUserID, int toUserID = -1)
		{
			User receiver;
			if (toUserID == -1)
			{
				receiver = uOW.UserRepo.Get().Where(e => e.RoleId == (int)AvailableRoles.Support).First();
			} else
			{
				receiver = uOW.UserRepo.GetByID(toUserID);
			}

			var msg = new SupportMessage()
			{
				Message = message,
				Sender = uOW.UserRepo.GetByID(fromUserID),
				Receiver = receiver,
				SendTime = DateTime.UtcNow
			};

			uOW.SupportRepo.Insert(msg);
			uOW.Save();
		}
	}
}
