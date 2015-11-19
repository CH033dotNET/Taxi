using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;
using Model;

namespace BAL.Manager
{
	public class DistrictManager : BaseManager, IDistrictManager
	{
		public DistrictManager(IUnitOfWork uOW)
			: base(uOW)
		{

		}
		/// <summary>
		/// Managaer method that adds new district entry to Db
		/// </summary>
		/// <param name="dName">parameter that represents a name property of a new object</param>
		public string addDistrict(string dName)
		{
			District newD = new Model.District { Name = dName };
			if (uOW.DistrictRepo.All.Where(x => x.Name.Equals(newD.Name, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
			{
				uOW.DistrictRepo.Insert(newD);
				uOW.Save();
				return "Success";
			}
			else { return "Error"; }
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
		public IEnumerable<District> getDistricts()
		{
			var list = uOW.DistrictRepo.Get(s => s.Deleted == false).ToList();
			return list;
		}

		public IEnumerable<District> GetSortedDistricts(string parameter)
		{
			switch (parameter)
			{
				case "name":
					var districtList = uOW.DistrictRepo.All.Where(x => x.Deleted == false).ToList().OrderBy(x => x.Name);
					return districtList;
				case "name_desc":
					districtList = uOW.DistrictRepo.All.Where(x => x.Deleted == false).ToList().OrderByDescending(x => x.Name);
					return districtList;
				default:
					districtList = uOW.DistrictRepo.All.Where(x => x.Deleted == false).ToList().OrderBy(x => x.Name);
					return districtList;
			}
		}

		public IEnumerable<District> GetSortedDeletedDistrictsBy(string parameter)
		{
			switch (parameter)
			{
				case "name":
					var deletedDistrictList = uOW.DistrictRepo.All.Where(s => s.Deleted == true).ToList().OrderBy(x => x.Name);
					return deletedDistrictList;
				case "name_desc":
					deletedDistrictList = uOW.DistrictRepo.All.Where(s => s.Deleted == true).ToList().OrderByDescending(x => x.Name);
					return deletedDistrictList;
				default:
					deletedDistrictList = uOW.DistrictRepo.All.Where(s => s.Deleted == true).ToList().OrderBy(x => x.Name);
					return deletedDistrictList;
			}
		}

		public IEnumerable<District> searchDistricts(string parameter)
		{
			//students = students.Where(s => s.LastName.Contains(searchString)
			//				   || s.FirstMidName.Contains(searchString));
			var result = uOW.DistrictRepo.All.Where(s => s.Deleted == false & (s.Name.StartsWith(parameter) || s.Name.Contains(parameter)));
			return result;
		}

		public IEnumerable<District> searchDeletedDistricts(string parameter)
		{

			var result = uOW.DistrictRepo.All.Where(s => s.Deleted == true & (s.Name.StartsWith(parameter) || s.Name.Contains(parameter)));
			return result;
		}

		public IEnumerable<District> searchAndSortDistricts(string search, string sort) 
		{
			switch (sort)
			{
				case "name":
					var DistrictList = uOW.DistrictRepo.All.Where(s => s.Deleted == false & (s.Name.StartsWith(search) || s.Name.Contains(search))).ToList().OrderBy(x => x.Name);
					return DistrictList;
				case "name_desc":
					DistrictList = uOW.DistrictRepo.All.Where(s => s.Deleted == false & (s.Name.StartsWith(search) || s.Name.Contains(search))).ToList().OrderByDescending(x => x.Name);
					return DistrictList;
				default:
					DistrictList = uOW.DistrictRepo.All.Where(s => s.Deleted == false & (s.Name.StartsWith(search) || s.Name.Contains(search))).ToList().OrderBy(x => x.Name);
					return DistrictList;
			}
		}

		public IEnumerable<Model.District> searchAndSortDeletedDistricts(string search, string sort)
		{
			switch (sort)
			{
				case "name":
					var deletedDistrictList = uOW.DistrictRepo.All.Where(s => s.Deleted == true & (s.Name.StartsWith(search) || s.Name.Contains(search))).ToList().OrderBy(x => x.Name);
					return deletedDistrictList;
				case "name_desc":
					deletedDistrictList = uOW.DistrictRepo.All.Where(s => s.Deleted == true & (s.Name.StartsWith(search) || s.Name.Contains(search))).ToList().OrderByDescending(x => x.Name);
					return deletedDistrictList;
				default:
					deletedDistrictList = uOW.DistrictRepo.All.Where(s => s.Deleted == true & (s.Name.StartsWith(search) || s.Name.Contains(search))).ToList().OrderBy(x => x.Name);
					return deletedDistrictList;
			}
		}

		/// <summary>
		/// Managaer method that gets one district entry which id property value matches input parameter.
		/// </summary>
		/// <param name="id">Input parameter that represents id property value.</param>
		/// <returns></returns>
		public District getById(int id)
		{
			if (id <= 0) { return null; }
			return uOW.DistrictRepo.GetByID(id);
		}
		/// <summary>
		/// Managaer method that gets one district entry which id property value matches input parameter.
		/// </summary>
		/// <param name="id">Input parameter that represents id property value.</param>
		/// <returns></returns>
		public District getOneDistrictByItsID(int id)
		{
			if (id <= 0) { return null; }
			var getDistrict = uOW.DistrictRepo.Get(s => s.Id == id).FirstOrDefault();
			if (getDistrict != null)
			{
				return getDistrict;
			}
			return null;
		}
		/// <summary>
		/// Manager method that edits specific district entry which id property value matches input parameters id property value
		/// </summary>
		/// <param name="district">Input parameter that represents object of type District </param>
		/// <returns></returns>
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
		/// <summary>
		///  Managaer method that gets one district entry which name property value matches input parameter.
		/// </summary>
		/// <param name="name">Input parameter that represents name property value.</param>
		/// <returns></returns>
		public District getByName(string name)
		{
			if (name == "") { return null; }
			return uOW.DistrictRepo.Get(s => s.Name == name).FirstOrDefault();
		}
		/// <summary>
		/// Managaer method that gets new list of deleted district entries. 
		/// These entries doesnt have Deleted property set to true.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<District> getDeletedDistricts()
		{
			var deletedDistricts = uOW.DistrictRepo.Get(s => s.Deleted == true).ToList();
			if (deletedDistricts != null)
			{
				return deletedDistricts;
			}
			return null;
		}
		/// <summary>
		/// Manager method has restoring function. It changes specific district entry Deleted property to false. 
		/// It chooses required entry by input parameter.
		/// </summary>
		/// <param name="Id">Input parameter that represents id property value.</param>
		/// <returns></returns>
		public District RestoreDistrict(int Id)
		{
			if (Id <= 0) { return null; }
			var deletedDistrict = uOW.DistrictRepo.GetByID(Id);
			if (deletedDistrict == null)
			{
				return null;
			}
			var restoredDistrict = SetStateRestored(deletedDistrict);
			return restoredDistrict;
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
			uOW.DistrictRepo.SetStateModified(oldDistrict);
			oldDistrict.Name = inputDistrict.Name;
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
		public District SetDistrictDeleted(int Id, string Name)
		{
			if (Id <= 0 || Name == "") { return null; }
			var districtToDelete = uOW.DistrictRepo.Get(s => s.Id == Id & s.Name == Name & s.Deleted == false).FirstOrDefault();
			if (districtToDelete == null)
			{
				return null;
			}
			var deletedDistrict = SetStateDeleted(districtToDelete);
			return deletedDistrict;
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

		public IQueryable<District> GetIQueryableDistricts()
		{
			return uOW.DistrictRepo.All;
		}
	}
}
