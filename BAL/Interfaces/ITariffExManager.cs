﻿using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
	public interface ITariffExManager
	{
		IEnumerable<TariffExDTO> GetAllTariffs();
		TariffExDTO GetTariffData(int id);
		TariffExDTO GetStandardTariff();
        bool SaveTariff(TariffExDTO tariff);
		bool DeleteTariff(int id);
		bool EnableTariff(int id);
	}
}
