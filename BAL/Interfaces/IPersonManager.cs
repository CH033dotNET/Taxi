using System;
using Model.DTO;

namespace BAL.Manager
{
	 public interface IPersonManager
	{
		PersonDTO DefaultImage(int UserId);
		void DeletePersonByID(int? id);
		PersonDTO EditPerson(PersonDTO person);
		PersonDTO GetPersonByPersonID(int? id);
		PersonDTO GetPersonByUserId(int? id);
		System.Collections.Generic.IEnumerable<PersonDTO> GetPersons();
		PersonDTO InsertPerson(PersonDTO person);
		void UpdatePhoneFMLnames(PersonDTO person);
	}
}
