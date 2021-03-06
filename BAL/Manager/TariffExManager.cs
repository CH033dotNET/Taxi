﻿using AutoMapper;
using BAL.Interfaces;
using DAL.Interface;
using Model.DB;
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

		public TariffExDTO GetStandardTariff()
		{
			return Mapper.Map<TariffExDTO>(uOW.TariffExRepo.All.Where(t => t.Name == "Standart").FirstOrDefault());
		}

		public bool SaveTariff(TariffExDTO tariff)
		{
			try
			{
				if (tariff.Id == -1)
				{
					var newTariff = Mapper.Map<TariffEx>(tariff);
					uOW.TariffExRepo.Insert(newTariff);
				}
				else
				{
					var tariffDb = uOW.TariffExRepo.GetByID(tariff.Id);

					tariffDb.Name = tariff.Name;
					tariffDb.Description = tariff.Description;
					tariffDb.PriceInCity = tariff.PriceInCity;
					tariffDb.PriceOutCity = tariff.PriceOutCity;
					tariffDb.PricePreOrder = tariff.PricePreOrder;
					tariffDb.PriceRegularCar = tariff.PriceRegularCar;
					tariffDb.PriceMinivanCar = tariff.PriceMinivanCar;
					tariffDb.PriceLuxCar = tariff.PriceLuxCar;
					tariffDb.PriceCourierOption = tariff.PriceCourierOption;
					tariffDb.PricePlateOption = tariff.PricePlateOption;
					tariffDb.PriceClientCarOption = tariff.PriceClientCarOption;
					tariffDb.PriceSpeakEnglishOption = tariff.PriceSpeakEnglishOption;
					tariffDb.PricePassengerSmokerOption = tariff.PricePassengerSmokerOption;
				}

				uOW.Save();
				return true;
			} catch (Exception ex)
			{
				return false;
			}
		}

		public bool DeleteTariff(int id)
		{
			try
			{
				var tariffDb = uOW.TariffExRepo.GetByID(id);
				tariffDb.Status = Common.Enum.TariffExStatus.Deleted;
				uOW.Save();
				return true;
			} catch (Exception ex)
			{
				return false;
			}
		}

		public bool EnableTariff(int id)
		{
			try
			{
				var tariffDb = uOW.TariffExRepo.GetByID(id);
				tariffDb.Status = Common.Enum.TariffExStatus.Active;
				uOW.Save();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}
