using System;
namespace BAL.Manager
{
	public interface IDistrictManager
	{
		void addDistrict(string dName);
		void deleteById(int id);
		Model.District EditDistrict(Model.District district);
		Model.District getById(int id);
		Model.District getByName(string name);
		System.Collections.Generic.IEnumerable<Model.District> getDeletedDistricts();
		System.Collections.Generic.IEnumerable<Model.District> getDistricts();
		Model.District getOneDistrictByItsID(int? id);
		Model.District RestoreDistrict(int Id);
		Model.District SetDistrictDeleted(int Id, string Name);
	}
}
