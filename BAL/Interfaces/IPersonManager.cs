using System;
namespace BAL.Manager
{
	 public interface IPersonManager
	{
		Model.DTO.PersonDTO DefaultImage(int UserId);
		void DeletePersonByID(int? id);
		Model.DTO.PersonDTO EditPerson(Model.DTO.PersonDTO person);
		Model.DTO.PersonDTO GetPersonByPersonID(int? id);
		Model.DTO.PersonDTO GetPersonByUserId(int? id);
		System.Collections.Generic.IEnumerable<Model.DTO.PersonDTO> GetPersons();
		Model.DTO.PersonDTO InsertPerson(Model.DTO.PersonDTO person);
		void UpdatePhoneFMLnames(Model.DTO.PersonDTO person);
	}
}
