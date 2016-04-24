using AutoMapper;
using BAL.Interfaces;
using DAL.Interface;
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
			var messages = uOW.SupportRepo.Get()
										//.Where(e => e.Sender.Id == userId || e.Receiver.Id == userId)
										.Select(s => Mapper.Map<SupportMessageDTO>(s));
			return messages;
		}
	}
}
