using System;
using System.Linq;
using System.Collections.Generic;

namespace BAL.Manager
{
	public interface IDistrictManager
	{
		string addDistrict(string dName);
		string deleteById(int id);
		Model.District EditDistrict(Model.District district);
		Model.District getById(int id);
		Model.District getByName(string name);
		IEnumerable<Model.District> getDeletedDistricts();
		IEnumerable<Model.District> getDistricts();
		Model.District getOneDistrictByItsID(int id);
		Model.District RestoreDistrict(int Id);
		Model.District SetDistrictDeleted(int Id, string Name);
		IEnumerable<Model.District> GetSortedDistricts(string parameter);

		IEnumerable<Model.District> GetSortedDeletedDistrictsBy(string parameter);

		IEnumerable<Model.District> searchDistricts(string parameter);

		IEnumerable<Model.District> searchAndSortDistricts(string search, string sort);

		IEnumerable<Model.District> searchAndSortDeletedDistricts(string search, string sort);

		IEnumerable<Model.District> searchDeletedDistricts(string parameter);

		IQueryable<Model.District> GetIQueryableDistricts();
	}
}
