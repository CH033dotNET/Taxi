using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Interface;
using Model.DB;
using Model.DTO;

namespace BAL.Manager
{
	 public class PersonManager : BaseManager
	{
		 public PersonManager(IUnitOfWork uOW) : base(uOW)
		 {
		 }

		 public PersonDTO InsertPerson(PersonDTO person)
		 {
			 var temp = Mapper.Map<Person>(person);
			 person.FirstName = temp.FirstName;
			 person.MiddleName = temp.MiddleName;
			 person.LastName = temp.LastName;
			 person.Phone = temp.Phone;
			 person.UserId = temp.UserId;
			 person.ImageName = temp.ImageName;
			 uOW.PersonRepo.Insert(temp);
			 uOW.Save();

			 return Mapper.Map<PersonDTO>(temp);
		 }

		 public void DeletePersonByID(int? id)
		 {
			 Person person = uOW.PersonRepo.GetByID(id);
			 uOW.CarRepo.Delete(person);
			 uOW.Save();
		 }

		 public PersonDTO EditPerson(PersonDTO person)
		 {
			 var newPerson = uOW.PersonRepo.Get(s => s.UserId == person.UserId).First();
			 if (newPerson == null)
			 {
				 return null;
			 }
			
			 uOW.PersonRepo.SetStateModified(newPerson);
			 newPerson.FirstName = person.FirstName;
			 newPerson.MiddleName = person.MiddleName;
			 newPerson.LastName = person.LastName;
			 newPerson.Phone = person.Phone;
			 newPerson.User.UserName = person.User.UserName;
			 newPerson.User.Email = person.User.Email;
			 newPerson.ImageName = person.ImageName;
			 
			 uOW.Save();
			 return Mapper.Map<PersonDTO>(newPerson);
		 }

		 public IEnumerable<PersonDTO> GetPersons()
		 {
			 var personList = uOW.PersonRepo.Get().Select(s => Mapper.Map<PersonDTO>(s));
			 return personList;
		 }

		 public PersonDTO GetPersonByUserId(int? id)
		 {
			 if (id == 0)
			 {
				 return null;
			 }
			 var person = uOW.PersonRepo.Get().Where(s => s.UserId == id).FirstOrDefault();
			 if (person != null)
			 {
				 return Mapper.Map<PersonDTO>(person);
			 }
			 return null;
		 }
		 public PersonDTO GetPersonByPersonID(int? id)
		 {
			 if (id == 0)
			 {
				 return null;
			 }
			 var person = uOW.PersonRepo.Get().Where(s => s.Id == id).FirstOrDefault();
			 if (person != null)
			 {
				 return Mapper.Map<PersonDTO>(person);
			 }
			 return null;
		 }
	}
}
