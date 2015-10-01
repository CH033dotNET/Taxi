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

    public class TarifManager : BaseManager
    {
        public TarifManager(IUnitOfWork uOW)
            : base(uOW)
        {
        }

        public IEnumerable<TarifDTO> GetTarifes()
        {
            var list = from tarifes in uOW.TarifRepo.Get()
                       select new TarifDTO
                       {
                           id = tarifes.id,
                           District = tarifes.District,
                           DistrictId = tarifes.DistrictId,
                           IsIntercity = tarifes.IsIntercity,
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

            if (temp.DistrictId == -1)
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
            temp.DistrictId = tarif.DistrictId==-1? null : tarif.DistrictId;
            temp.IsIntercity = tarif.IsIntercity;
            temp.MinimalPrice = tarif.MinimalPrice;
            temp.Name = tarif.Name;
            temp.OneMinuteCost = tarif.OneMinuteCost;
            temp.StartPrice = tarif.StartPrice;
            temp.WaitingCost = tarif.WaitingCost;
            uOW.TarifRepo.SetStateModified(temp);
            uOW.Save();
            return Mapper.Map<TarifDTO>(temp);
        }

        public void DeleteTarif(int tarifId)
        {

            uOW.TarifRepo.Delete(uOW.TarifRepo.GetByID(tarifId));
            // TODO:
            //uOW.UserInfoRepo.Delete(uOW.UserInfoRepo.GetByID(userId));
            uOW.Save();

        }

        public List<District> getDistrictsList()
        {
            List<District> list = new List<District>();
            list.Add(new District() { Id=-1, Name="[Select District]"});
            list.AddRange(uOW.DistrictRepo.Get());
            return list;
        }



    }

}
