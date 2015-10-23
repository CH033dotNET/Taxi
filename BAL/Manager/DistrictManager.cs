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
			var list = uOW.DistrictRepo.Get(s => s.Deleted == false).ToList();
			return list;
		}
		public District getById(int id)
		{
			return uOW.DistrictRepo.GetByID(id);
		}

		public District getOneDistrictByItsID(int? id)
		{
			var getDistrict = uOW.DistrictRepo.Get(s => s.Id == id).FirstOrDefault();
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

        public District getByName(string name)
        {
			return uOW.DistrictRepo.Get(s => s.Name == name).FirstOrDefault();
        }

		public IEnumerable<District> getDeletedDistricts()
		{
			var deletedDistricts = uOW.DistrictRepo.Get(s => s.Deleted == true).ToList();
			if (deletedDistricts != null)
			{
				return deletedDistricts;
			}
			return null;
		}
		public District RestoreDistrict(int Id)
		{
			var deletedDistrict = uOW.DistrictRepo.GetByID(Id);
			if (deletedDistrict == null)
			{
				return null;
			}
			var restoredDistrict = SetStateRestored(deletedDistrict);
			return deletedDistrict;
		}

		private District SetDistrictStateModified(District oldDistrict, District inputDistrict)
		{
			uOW.DistrictRepo.SetStateModified(oldDistrict);
			oldDistrict.Name = inputDistrict.Name;
			uOW.Save();
			return oldDistrict;
		}

		public District SetDistrictDeleted(int Id, string Name)
		{
			var districtToDelete = uOW.DistrictRepo.Get(s => s.Id == Id & s.Name == Name/* & s.Deleted == false*/).FirstOrDefault();
			if (districtToDelete == null)
			{
				return null;
			}
			var deletedDistrict = SetStateDeleted(districtToDelete);
			return deletedDistrict;
		}

		private District SetStateDeleted(District district)
		{
			uOW.DistrictRepo.SetStateModified(district);
			district.Deleted = true;
			uOW.Save();
			return district;
		}

		private District SetStateRestored(District district)
		{
			uOW.DistrictRepo.SetStateModified(district);
			district.Deleted = false;
			uOW.Save();
			return district;
		}
	}
}
