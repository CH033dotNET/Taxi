using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;
using Model;
using System.Data.Entity;
using Model.DTO;
using AutoMapper;

namespace BAL.Manager
{
	public class DistrictManager : BaseManager, IDistrictManager
	{
		public DistrictManager(IUnitOfWork uOW)
			: base(uOW)
		{
			Mapper.CreateMap<Coordinate, CoordinateDTO>();
			Mapper.CreateMap<District, DistrictDTO>();
			Mapper.CreateMap<CoordinateDTO, Coordinate>();
			Mapper.CreateMap<DistrictDTO, District>();
		}
		/// <summary>
		/// Managaer method that adds new district entry to Db
		/// </summary>
		/// <param name="district">parameter that represents a new object</param>
		public DistrictDTO addDistrict(DistrictDTO district)
		{
			if (uOW.DistrictRepo.All.Where(x => x.Name.Equals(district.Name, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
			{
				var model = Mapper.Map<District>(district);
				uOW.DistrictRepo.Insert(model);
				uOW.Save();
				var newDistrict = uOW.DistrictRepo.All.Where(d => d.Name == district.Name).Include(c => c.Coordinates).FirstOrDefault();
				return Mapper.Map<DistrictDTO>(newDistrict);
			}
			else { return null; }
		}
		/// <summary>
		/// Managaer method that deletes a specific district enty with id matching id parameter
		/// </summary>
		/// <param name="id">Input parameter thats represents specific district id</param>
		public string deleteById(int id)
		{
			if (id <= 0)
			{
				return "Error";
			}
			else
			{
				District a = uOW.DistrictRepo.GetByID(id);
				uOW.DistrictRepo.Delete(a);
				uOW.Save();
				return "Success";
			}
		}
		/// <summary>
		/// Managaer method that gets new list of avialable district entries. These entries doesnt have Deleted property set to true.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<DistrictDTO> getDistricts()
		{
			var districts = uOW.DistrictRepo.All.Include(c => c.Coordinates).Where(d => d.Deleted == false).ToList();
			SortCoordinates(districts);
			var res = Mapper.Map<IEnumerable<DistrictDTO>>(districts);
			return res;
		}

		public IEnumerable<DistrictDTO> GetSortedDistricts(string parameter)
		{
			List<District> districtList = null;
			var districts = uOW.DistrictRepo.All.Where(x => x.Deleted == false).Include(c => c.Coordinates).ToList();
			switch (parameter)
			{
				case "name":
					districtList = districts.OrderBy(x => x.Name).ToList();
					break;
				case "name_desc":
					districtList = districts.OrderByDescending(x => x.Name).ToList();
					break;
				default:
					districtList = districts.OrderBy(x => x.Name).ToList();
					break;
			}
			SortCoordinates(districtList);
			return Mapper.Map<IEnumerable<DistrictDTO>>(districtList);
		}

		public IEnumerable<DistrictDTO> GetSortedDeletedDistrictsBy(string parameter)
		{
			List<District> deletedDistrictList = null;
			var districts = uOW.DistrictRepo.All.Where(s => s.Deleted == true).Include(c => c.Coordinates).ToList();
			switch (parameter)
			{
				case "name":
					deletedDistrictList = districts.OrderBy(x => x.Name).ToList();
					break;
				case "name_desc":
					deletedDistrictList = districts.OrderByDescending(x => x.Name).ToList();
					break;
				default:
					deletedDistrictList = districts.OrderBy(x => x.Name).ToList();
					break;
			}
			SortCoordinates(deletedDistrictList);
			return Mapper.Map<IEnumerable<DistrictDTO>>(deletedDistrictList);
		}

		public IEnumerable<DistrictDTO> searchDistricts(string parameter)
		{
			//students = students.Where(s => s.LastName.Contains(searchString)
			//				   || s.FirstMidName.Contains(searchString));
			var result = uOW.DistrictRepo.All.Where(s => s.Deleted == false & (s.Name.StartsWith(parameter) || s.Name.Contains(parameter))).Include(c => c.Coordinates).ToList();
			SortCoordinates(result);
			return Mapper.Map<IEnumerable<DistrictDTO>>(result);
		}

		public IEnumerable<DistrictDTO> searchDeletedDistricts(string parameter)
		{

			var result = uOW.DistrictRepo.All.Where(s => s.Deleted == true & (s.Name.StartsWith(parameter) || s.Name.Contains(parameter))).Include(c => c.Coordinates).ToList();
			SortCoordinates(result);
			return Mapper.Map<IEnumerable<DistrictDTO>>(result);
		}

		public bool SetParent(int id, int? parentId)
		{
			var district = uOW.DistrictRepo.GetByID(id);
			district.ParentId = parentId;
			uOW.DistrictRepo.Update(district);
			uOW.Save();
			return true;
		}

		public IEnumerable<DistrictDTO> searchAndSortDistricts(string search, string sort)
		{
			var districts = uOW.DistrictRepo.All.Where(s => s.Deleted == false & (s.Name.StartsWith(search) || s.Name.Contains(search))).Include(c => c.Coordinates).ToList();
			switch (sort)
			{
				case "name":
					districts = districts.OrderBy(x => x.Name).ToList();
					break;
				case "name_desc":
					districts = districts.OrderByDescending(x => x.Name).ToList();
					break;
				default:
					districts = districts.OrderBy(x => x.Name).ToList();
					break;
			}SortCoordinates(districts);
			return Mapper.Map<IEnumerable<DistrictDTO>>(districts);
		}

		public IEnumerable<DistrictDTO> searchAndSortDeletedDistricts(string search, string sort)
		{
			var districts = uOW.DistrictRepo.All.Where(s => s.Deleted == false & (s.Name.StartsWith(search) || s.Name.Contains(search))).Include(c => c.Coordinates).ToList();
			switch (sort)
			{
				case "name":
					districts = uOW.DistrictRepo.All.Where(s => s.Deleted == true & (s.Name.StartsWith(search) || s.Name.Contains(search))).ToList().OrderBy(x => x.Name).ToList();
					break;
				case "name_desc":
					districts = uOW.DistrictRepo.All.Where(s => s.Deleted == true & (s.Name.StartsWith(search) || s.Name.Contains(search))).ToList().OrderByDescending(x => x.Name).ToList();
					break;
				default:
					districts = uOW.DistrictRepo.All.Where(s => s.Deleted == true & (s.Name.StartsWith(search) || s.Name.Contains(search))).ToList().OrderBy(x => x.Name).ToList();
					break;
			}
			return Mapper.Map<IEnumerable<DistrictDTO>>(districts);
		}

		/// <summary>
		/// Managaer method that gets one district entry which id property value matches input parameter.
		/// </summary>
		/// <param name="id">Input parameter that represents id property value.</param>
		/// <returns></returns>
		public DistrictDTO getById(int id)
		{
			if (id <= 0) { return null; }
			return Mapper.Map<DistrictDTO>(uOW.DistrictRepo.All.Where(s => s.Id == id).Include(c => c.Coordinates));
		}
		/// <summary>
		/// Managaer method that gets one district entry which id property value matches input parameter.
		/// </summary>
		/// <param name="id">Input parameter that represents id property value.</param>
		/// <returns></returns>
		public DistrictDTO getOneDistrictByItsID(int id)
		{
			if (id <= 0) { return null; }
			var getDistrict = uOW.DistrictRepo.All.Where(s => s.Id == id).Include(c => c.Coordinates).FirstOrDefault();
			if (getDistrict != null)
			{
				return Mapper.Map<DistrictDTO>(getDistrict);
			}
			return null;
		}
		/// <summary>
		/// Manager method that edits specific district entry which id property value matches input parameters id property value
		/// </summary>
		/// <param name="district">Input parameter that represents object of type District </param>
		/// <returns></returns>
		public DistrictDTO EditDistrict(DistrictDTO district)
		{
			var oldDistrict = uOW.DistrictRepo.All.Where(d => d.Id == district.Id).Include(c => c.Coordinates).FirstOrDefault();
			if (oldDistrict == null)
			{
				return null;
			}
			oldDistrict.Name = district.Name;
			var newCoord = Mapper.Map<List<Coordinate>>(district.Coordinates);
			newCoord.Reverse();
			foreach (var coord in newCoord)
			{
				var editCoord = oldDistrict.Coordinates.Find(c => c.Id == coord.Id);
				if (editCoord == null)
				{
					coord.District = oldDistrict;
					oldDistrict.Coordinates.Add(coord);
				}
				else
				{
					editCoord.Index = coord.Index;
					editCoord.Latitude = coord.Latitude;
					editCoord.Longitude = coord.Longitude;
				}
			}
			uOW.DistrictRepo.Update(oldDistrict);
			uOW.Save();
			var newDistrict = uOW.DistrictRepo.All.Where(d => d.Id == district.Id).FirstOrDefault();
			SortCoordinates(newDistrict);
			return Mapper.Map<DistrictDTO>(newDistrict);
		}
		/// <summary>
		///  Managaer method that gets one district entry which name property value matches input parameter.
		/// </summary>
		/// <param name="name">Input parameter that represents name property value.</param>
		/// <returns></returns>
		public DistrictDTO getByName(string name)
		{
			if (name == "") { return null; }
			return Mapper.Map<DistrictDTO>(uOW.DistrictRepo.All.Where(s => s.Name == name).Include(c => c.Coordinates).FirstOrDefault());
		}
		/// <summary>
		/// Managaer method that gets new list of deleted district entries. 
		/// These entries doesnt have Deleted property set to true.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<DistrictDTO> getDeletedDistricts()
		{
			var deletedDistricts = uOW.DistrictRepo.All.Where(s => s.Deleted == true).Include(c => c.Coordinates).ToList();
			if (deletedDistricts != null)
			{
				SortCoordinates(deletedDistricts);
				return Mapper.Map<List<DistrictDTO>>(deletedDistricts);
			}
			return null;
		}
		/// <summary>
		/// Manager method has restoring function. It changes specific district entry Deleted property to false. 
		/// It chooses required entry by input parameter.
		/// </summary>
		/// <param name="Id">Input parameter that represents id property value.</param>
		/// <returns></returns>
		public DistrictDTO RestoreDistrict(int Id)
		{
			if (Id <= 0) { return null; }
			var deletedDistrict = uOW.DistrictRepo.All.Include(d=>d.Coordinates).FirstOrDefault(d=>d.Id==Id);
			if (deletedDistrict == null)
			{
				return null;
			}
			var restoredDistrict = SetStateRestored(deletedDistrict);
			SortCoordinates(restoredDistrict);
			return Mapper.Map<DistrictDTO>(restoredDistrict);
		}
		/// <summary>
		/// Private managaer method that incapsulates edititng logic. Used by edit method, 
		/// that changes specific district name property.
		/// </summary>
		/// <param name="oldDistrict">Input parameter that represents old district entry that we need to change.</param>
		/// <param name="inputDistrict">Input parameter that represents new district entry that we use to change old entry.</param>
		/// <returns></returns>
		private District SetDistrictStateModified(District oldDistrict, District inputDistrict)
		{
			oldDistrict.Name = inputDistrict.Name;
			oldDistrict.Coordinates = inputDistrict.Coordinates;
			uOW.DistrictRepo.SetStateModified(oldDistrict);
			uOW.Save();
			return oldDistrict;
		}
		/// <summary>
		/// Manager method we use to delete specific district entry. 
		/// District entry will not be deleted completely, only iit`s Deleted property will be set to true. 
		/// </summary>
		/// <param name="Id">Input parameter that represents id property value.</param>
		/// <param name="Name">Input parameter that represents name property value.</param>
		/// <returns></returns>
		public bool SetDistrictDeleted(int id)
		{
			var districtToDelete = GetAllChildren(id);
			if (districtToDelete.Count == 0)
			{
				return false;
			}
			foreach(var district in districtToDelete)
			{
				SetStateDeleted(district);
			}

			return true;
		}
		/// <summary>
		/// Private manager method that incapsulates all logic we need to set specific district entry to deleted state.
		/// </summary>
		/// <param name="district">Input paramater that represents object that we need to change.</param>
		/// <returns></returns>
		private District SetStateDeleted(District district)
		{
			uOW.DistrictRepo.SetStateModified(district);
			district.Deleted = true;
			district.ParentId = null;
			uOW.Save();
			return district;
		}
		/// <summary>
		/// Manager method that is used to set specific district object to avialable state. 
		/// It changes object`s Deleted property to false.
		/// </summary>
		/// <param name="district"></param>
		/// <returns></returns>
		private District SetStateRestored(District district)
		{
			uOW.DistrictRepo.SetStateModified(district);
			district.Deleted = false;
			uOW.Save();
			return district;
		}

		private void SortCoordinates(List<District> districts)
		{
			districts.ForEach(d => d.Coordinates = d.Coordinates.OrderBy(c => c.Index).ToList());
		}
		private void SortCoordinates(District district)
		{
			district.Coordinates = district.Coordinates.OrderBy(c => c.Index).ToList();
		}

		public IQueryable<DistrictDTO> GetIQueryableDistricts()
		{
			return Mapper.Map<IQueryable<DistrictDTO>>(uOW.DistrictRepo.All);
		}

		private List<District> GetAllChildren(int id)
		{
			var children = new List<District>();
			var current = uOW.DistrictRepo.GetByID(id);
			if (current.IsFolder)
			{
				var currentChildren = uOW.DistrictRepo.All.Where(d => d.ParentId == id).ToList();
				foreach(var child in currentChildren)
				{
					children.AddRange(GetAllChildren(child.Id));
				}
				children.Add(current);
			}
			else
			{
				children.Add(current);
			}
			return children;
		}

		public IEnumerable<DistrictDTO> GetFilesDistricts()
		{
			return Mapper.Map<List<DistrictDTO>>(uOW.DistrictRepo
																.All.
																Where(d => d.Deleted == false && d.IsFolder == false).
																Include(d => d.Coordinates).
																ToList());
;		}
	}
}
