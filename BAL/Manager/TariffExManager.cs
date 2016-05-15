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
	public class TariffExManager : BaseManager, ITariffExManager
	{
		public TariffExManager(IUnitOfWork uOW) : base(uOW)
		{

		}

		public IEnumerable<TariffExDTO> GetAllTariffs()
		{
			var tariffs = uOW.TariffExRepo.All.ToList();
			return tariffs.Select(x => Mapper.Map<TariffExDTO>(x));
		}

		public TariffExDTO GetTariffData(int id)
		{
			var tariff = uOW.TariffExRepo.GetByID(id);
			return Mapper.Map<TariffExDTO>(tariff);
		}
	}
}
