﻿using System;
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
            Mapper.CreateMap<AddressDTO, UserAddress>();
            Mapper.CreateMap<UserAddress, AddressDTO>();
			Mapper.CreateMap<Person, PersonDTO>();
			Mapper.CreateMap<PersonDTO, Person>();
            Mapper.CreateMap<Location, LocationDTO>();
            Mapper.CreateMap<LocationDTO, Location>();
			Mapper.CreateMap<WorkshiftHistory, WorkshiftHistoryDTO>();
			Mapper.CreateMap<WorkshiftHistoryDTO, WorkshiftHistory>();
            Mapper.CreateMap<TarifDTO, Tarif>();
			Mapper.CreateMap<Tarif, TarifDTO>();
			Mapper.CreateMap<CoordinatesDTO, Coordinates>();
			Mapper.CreateMap<Coordinates, CoordinatesDTO>();
			Mapper.CreateMap<Order, OrderDTO>();
			Mapper.CreateMap<OrderDTO, Order>();
			Mapper.CreateMap<WorkerStatus, WorkerStatusDTO>();
			Mapper.CreateMap<WorkerStatusDTO, WorkerStatus>();
			Mapper.CreateMap<SupportMessageDTO, SupportMessage>();
			Mapper.CreateMap<SupportMessage, SupportMessageDTO>();
			Mapper.CreateMap<TariffExDTO, TariffEx>();
			Mapper.CreateMap<TariffEx, TariffExDTO>();
		}
	}
}
