﻿using System;
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
