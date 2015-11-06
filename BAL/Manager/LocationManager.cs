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
		public LocationDTO getByDistrictId(int id)
		{
			var item = uOW.LocationRepo.Get().Where(s => s.DistrictId == id).FirstOrDefault();
			if (item != null)
			{
				return Mapper.Map<LocationDTO>(item);
			}
			return null;
		}
		public void DeleteLocation(int UserId)
		{
			uOW.LocationRepo.Delete(uOW.LocationRepo.GetByID(UserId));
			uOW.Save();
			return;
		}
		public List<DriverDistrictInfoDTO> GetDriverDistrictInfo()
		{
			List<DriverDistrictInfoDTO> DDI = new List<DriverDistrictInfoDTO>();

			var Districts = uOW.DistrictRepo.Get();
			var Localizations = uOW.LocationRepo.Get();


			var query =
				from D in Districts
				join L in Localizations
				on D.Id equals L.DistrictId
				group D by new { D.Id, D.Name } into grouped
				select new DriverDistrictInfoDTO
				{
					DriverCount = grouped.Count(),
					DistrictName = grouped.Key.Name,
					DistrictId = grouped.Key.Id
				};


			foreach (var info in query)
			{
				var Id = getByDistrictId(info.DistrictId);
				info.DriverId = Id.UserId;
				DDI.Add(info);

			}


			return DDI;
		}
	}

}
