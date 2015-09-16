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
			uOW.DistrictRepo.Insert(new Model.District { Name = dName });
			uOW.Save();
		}
		public void deleteDistrictById(int id)
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
	}
}
