using System;
using System.Collections.Generic;
using Model.DTO;

namespace BAL.Manager
{
	public interface ITarifManager
	{
		TarifDTO AddTarif(TarifDTO tarifDTO);
		void DeleteTarif(int tarifId);
		TarifDTO GetById(int id);
		List<Model.District> getDistrictsList();
		IEnumerable<TarifDTO> GetTarifes();
		TarifDTO UpdateTarif(TarifDTO tarif);
	}
}
