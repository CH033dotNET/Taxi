using AutoMapper;
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

    public class TarifManager : BaseManager, ITarifManager
    {

        public TarifManager(IUnitOfWork uOW)
			:base(uOW)
        {

        }

        public IEnumerable<TarifDTO> GetTarifes()
        {
            var list = from tarifes in uOW.TarifRepo.All
                       select new TarifDTO
                       {
                           id = tarifes.id,
                           District = tarifes.District,
                           DistrictId = tarifes.DistrictId,
                           IsIntercity = tarifes.IsIntercity,
                           IsStandart = tarifes.IsStandart,
                           MinimalPrice = tarifes.MinimalPrice,
                           Name = tarifes.Name,
                           OneMinuteCost = tarifes.OneMinuteCost,
                           StartPrice = tarifes.StartPrice,
                           WaitingCost = tarifes.WaitingCost
                       };

            return list.ToList();
        }

        public TarifDTO AddTarif(TarifDTO tarifDTO)
        {
            var temp = Mapper.Map<Tarif>(tarifDTO);
            temp.Name = temp.Name.Trim();

            if (temp.IsIntercity || temp.IsStandart)
                temp.DistrictId = null;

            uOW.TarifRepo.Insert(temp);
            uOW.Save();
            return Mapper.Map<TarifDTO>(temp);
        }

        public TarifDTO GetById(int id)
        {
            return Mapper.Map<TarifDTO>(uOW.TarifRepo.All.FirstOrDefault(x => x.id == id));
        }

        public TarifDTO UpdateTarif(TarifDTO tarif)
        {
            var temp = uOW.TarifRepo.Get(u => u.id == tarif.id).First();
            if (temp == null)
            {
                return null;
            }
            /*if (IsAdministratorById(temp.Id))
            {
                return null;
            }*/

            temp.District = tarif.District;
            temp.DistrictId = tarif.IsIntercity || tarif.IsStandart ? null : tarif.DistrictId;
            temp.IsIntercity = tarif.IsIntercity;
            temp.IsStandart = tarif.IsStandart;
            temp.MinimalPrice = tarif.MinimalPrice;
            temp.Name = tarif.Name;
            temp.OneMinuteCost = tarif.OneMinuteCost;
            temp.StartPrice = tarif.StartPrice;
            temp.WaitingCost = tarif.WaitingCost;
            uOW.TarifRepo.SetStateModified(temp);
            uOW.Save();
            return Mapper.Map<TarifDTO>(temp);
        }
		//TODO:
		//need more tests
        public void DeleteTarif(int tarifId)
        {
			var a = uOW.TarifRepo.All.Where(x => x.id == tarifId && x.IsStandart == false).FirstOrDefault();

			if (a != null)
			{
				var cordinates = uOW.CoordinatesHistoryRepo.All.Where(x => x.TarifId == tarifId);
				foreach(var item in cordinates)
					uOW.CoordinatesHistoryRepo.Delete(item);
				
				uOW.TarifRepo.Delete(a);
				
				uOW.Save();
			}

        }

        public List<District> getDistrictsList()
        {
            List<District> list = uOW.DistrictRepo.Get().ToList();
            return list;
        }



    }

}
