using System;
using System.Linq;
using System.Collections.Generic;
using Model.DTO;

namespace BAL.Manager
{
	public interface IDistrictManager
	{
		DistrictDTO addDistrict(DistrictDTO district);
		string deleteById(int id);
		DistrictDTO EditDistrict(DistrictDTO district);
		DistrictDTO getById(int id);
		IEnumerable<DistrictDTO> getDeletedDistricts();
		IEnumerable<DistrictDTO> getDistricts();
		IEnumerable<DistrictDTO> GetFilesDistricts();
		DistrictDTO RestoreDistrict(int Id);
		bool SetDistrictDeleted(int id);

		IEnumerable<DistrictDTO> GetSortedDeletedDistrictsBy(string parameter);

		IEnumerable<DistrictDTO> searchDistricts(string parameter);

		IEnumerable<DistrictDTO> searchAndSortDeletedDistricts(string search, string sort);

		IEnumerable<DistrictDTO> searchDeletedDistricts(string parameter);

		IQueryable<DistrictDTO> GetIQueryableDistricts();

		bool SetParent(int id, int? parentId);
	}
}
