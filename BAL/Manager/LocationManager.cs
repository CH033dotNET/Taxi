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
	public class LocationManager : BaseManager, ILocationManager
	{
		
		public LocationManager(IUnitOfWork uOW)
			:base(uOW)
		{

		}
		public LocationDTO GetByUserId(int id)
		{
			var item = uOW.LocationRepo.Get().Where(s => s.UserId == id)
				.FirstOrDefault();

			if (item != null)
			{
				return Mapper.Map<LocationDTO>(item);
			}
			return null;
		}



		public LocationDTO AddLocation(LocationDTO local)
		{

			var temp = Mapper.Map<Location>(local);
			uOW.LocationRepo.Insert(temp);
			uOW.Save();
			return Mapper.Map<LocationDTO>(temp);
		}
		public LocationDTO UpdateLocation(LocationDTO local)
		{
			var temp = uOW.LocationRepo.Get(u => u.UserId == local.UserId).FirstOrDefault();
			if (temp == null)
			{
				return null;
			}
			uOW.LocationRepo.SetStateModified(temp);
			temp.DistrictId = local.DistrictId;
			uOW.Save();
			return Mapper.Map<LocationDTO>(temp);
		}
		public void DeleteLocation(int UserId)
		{
			uOW.LocationRepo.Delete(uOW.LocationRepo.GetByID(UserId));
			uOW.Save();
			return;
		}
		public List<DriverDistrictInfoDTO> GetDriverDistrictInfo(int userId)
		{
			int currentDistrict = uOW.LocationRepo.All.Where(x => x.UserId == userId).Select(x => x.DistrictId).FirstOrDefault();
			
			var tet = uOW.DistrictRepo.All.Where(x => !(x.Deleted)).Select(x =>
				new DriverDistrictInfoDTO() { 
					DriverCount = uOW.LocationRepo.All.Where(y => y.DistrictId == x.Id).Count(),
					DistrictName = x.Name,
					DistrictId = x.Id,
					ThoseDriver = x.Id == currentDistrict
				});
			return tet.ToList();
		}
	}

}
