﻿using System;
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
		DistrictDTO getByName(string name);
		IEnumerable<DistrictDTO> getDeletedDistricts();
		IEnumerable<DistrictDTO> getDistricts();
		IEnumerable<DistrictDTO> GetFilesDistricts();
		DistrictDTO getOneDistrictByItsID(int id);
		DistrictDTO RestoreDistrict(int Id);
		bool SetDistrictDeleted(int id);
		IEnumerable<DistrictDTO> GetSortedDistricts(string parameter);

		IEnumerable<DistrictDTO> GetSortedDeletedDistrictsBy(string parameter);

		IEnumerable<DistrictDTO> searchDistricts(string parameter);

		IEnumerable<DistrictDTO> searchAndSortDistricts(string search, string sort);

		IEnumerable<DistrictDTO> searchAndSortDeletedDistricts(string search, string sort);

		IEnumerable<DistrictDTO> searchDeletedDistricts(string parameter);

		IQueryable<DistrictDTO> GetIQueryableDistricts();

		bool SetParent(int id, int? parentId);
	}
}
