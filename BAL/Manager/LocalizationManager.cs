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
    public class LocalizationManager : BaseManager
    {
        public LocalizationManager(IUnitOfWork uOW)
            : base(uOW)
        {
        }
        public LocalizationDTO GetByUserId(int id)
        {
            var item = uOW.LocalizationRepo.Get().Where(s => s.UserId == id)
                .FirstOrDefault();

            if (item != null)
            {
                return Mapper.Map<LocalizationDTO>(item);
            }
            return null;
        }



        public LocalizationDTO AddLoc(LocalizationDTO local)
        {

            var temp = Mapper.Map<Localization>(local);
            uOW.LocalizationRepo.Insert(temp);
            uOW.Save();
            return Mapper.Map<LocalizationDTO>(temp);
        }
        public LocalizationDTO UpdateLocalization(LocalizationDTO local)
        {
            var temp = uOW.LocalizationRepo.Get(u => u.UserId == local.UserId).First();
            if (temp == null)
            {
                return null;
            }
            uOW.LocalizationRepo.SetStateModified(temp);
            temp.DistrictId = local.DistrictId;
            uOW.Save();
            return Mapper.Map<LocalizationDTO>(temp);
        }
        public LocalizationDTO getByDistrictId(int id)
        {
            var item = uOW.LocalizationRepo.Get().Where(s => s.DistrictId == id).FirstOrDefault();
            if (item != null)
            {
                return Mapper.Map<LocalizationDTO>(item);
            }
            return null;
        }
    }

}
