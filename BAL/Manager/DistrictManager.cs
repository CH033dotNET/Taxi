using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;
using Model;

namespace BAL.Manager
{
	public class DistrictManager : BaseManager
	{
		public DistrictManager(IUnitOfWork uOW) : base(uOW)
		{
			
		}

		public void addDistrict(string dName)
		{
			if (dName.Length > 3)
			{
				District newD = new Model.District { Name = dName };
				if (uOW.DistrictRepo.All.Where(x => x.Name.Equals(newD.Name, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
				{

					uOW.DistrictRepo.Insert(newD);
					uOW.Save();
				}
			}
		}
		public void deleteById(int id)
		{
			District a = uOW.DistrictRepo.GetByID(id);
			uOW.DistrictRepo.Delete(a);
			uOW.Save();
		}
		public IEnumerable<District> getDistricts()
		{
			var list = uOW.DistrictRepo.Get();
			return list;
		}
		public District getById(int id)
		{
			return uOW.DistrictRepo.GetByID(id);
		}

		public District getOneDistrictByItsID(int? id)
		{
			var getDistrict = uOW.DistrictRepo.Get().Where(s => s.Id == id).FirstOrDefault();
			if (getDistrict != null)
			{
				return getDistrict;
			}
			return null;
		}

		public District EditDistrict(District district)
		{
			var oldDistrict = uOW.DistrictRepo.Get(s => s.Id == district.Id).FirstOrDefault();
			if (oldDistrict == null)
			{
				return null;
			}
			var newDistrict = SetDistrictStateModified(oldDistrict, district);
			return newDistrict;
		}

		private District SetDistrictStateModified(District oldDistrict, District inputDistrict)
		{
			uOW.DistrictRepo.SetStateModified(oldDistrict);
			oldDistrict.Name = inputDistrict.Name;
			uOW.Save();
			return oldDistrict;
		}

        public District getByName(string name)
        {
            return uOW.DistrictRepo.Get().Where(s => s.Name == name).FirstOrDefault();
        }
	}
}
