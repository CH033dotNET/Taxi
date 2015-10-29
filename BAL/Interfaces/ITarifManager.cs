using System;
namespace BAL.Manager
{
	public interface ITarifManager
	{
		Model.DTO.TarifDTO AddTarif(Model.DTO.TarifDTO tarifDTO);
		void DeleteTarif(int tarifId);
		Model.DTO.TarifDTO GetById(int id);
		System.Collections.Generic.List<Model.District> getDistrictsList();
		System.Collections.Generic.IEnumerable<Model.DTO.TarifDTO> GetTarifes();
		Model.DTO.TarifDTO UpdateTarif(Model.DTO.TarifDTO tarif);
	}
}
