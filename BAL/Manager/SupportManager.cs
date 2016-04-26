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

		public SupporterInfoDTO GetSupporter()
		{
			var user = uOW.UserRepo.Get().Where(e => e.RoleId == (int)AvailableRoles.Support).First();
			var person = uOW.PersonRepo.Get(e => e.UserId == user.Id).First();

			var info = new SupporterInfoDTO()
			{
				Id = user.Id,
				Name = person.FirstName,
				Photo = person.ImageName
			};

			return info;
		}

		public List<SupporterInfoDTO> GetChatUsers()
		{
			var allSenders = uOW.SupportRepo.Get()
								//.Get(e => e.SendTime > DateTime.UtcNow.AddMinutes(-30))
								.OrderBy(e => e.SendTime)
								.Select(e => e.SenderId)
								.Distinct();

			List<SupporterInfoDTO> persons = new List<SupporterInfoDTO>();
			foreach (int senderId in allSenders)
			{
				var person = uOW.PersonRepo.Get(e => e.UserId == senderId).First();
				var info = new SupporterInfoDTO()
				{
					Id = senderId,
					Name = person.FirstName,
					Photo = person.ImageName
				};

				persons.Add(info);
			}

			return persons;
		}

		public IEnumerable<SupportMessageDTO> GetMessages(int userId)
		{
			var messages = uOW.SupportRepo
							.Get(e => e.Receiver.Id == userId || e.Sender.Id == userId)
							.Select(s => Mapper.Map<SupportMessageDTO>(s));

			return messages;
		}

		public void SendMessage(string message, int fromUserID, int toUserID)
		{
			var msg = new SupportMessage()
			{
				Message = message,
				Sender = uOW.UserRepo.GetByID(fromUserID),
				Receiver = uOW.UserRepo.GetByID(toUserID),
				SendTime = DateTime.UtcNow
			};

			uOW.SupportRepo.Insert(msg);
			uOW.Save();
		}
	}
}
