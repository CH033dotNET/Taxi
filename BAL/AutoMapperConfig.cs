using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Model;
using Model.DTO;
using Model.DB;

namespace Common
{
	public class AutoMapperConfig
	{
		public static void Configure()
		{
			Mapper.CreateMap<User, UserDTO>();
			Mapper.CreateMap<UserDTO, User>();
			Mapper.CreateMap<Car, CarDTO>();
			Mapper.CreateMap<CarDTO, Car>();
            Mapper.CreateMap<UserAddress, AddressDTO>();
            Mapper.CreateMap<AddressDTO, UserAddress>();
			Mapper.CreateMap<Person, PersonDTO>();
			Mapper.CreateMap<PersonDTO, Person>();
            Mapper.CreateMap<Localization, LocalizationDTO>();
            Mapper.CreateMap<LocalizationDTO, Localization>();
			Mapper.CreateMap<WorkshiftHistory, WorkshiftHistoryDTO>();
			Mapper.CreateMap<WorkshiftHistoryDTO, WorkshiftHistory>();
		}
	}
}
