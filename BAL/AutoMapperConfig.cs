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
			Mapper.CreateMap<Coordinate, CoordinateDTO>();
			Mapper.CreateMap<District, DistrictDTO>();
			Mapper.CreateMap<CoordinateDTO, Coordinate>();
			Mapper.CreateMap<DistrictDTO, District>();
			Mapper.CreateMap<CoordinatesEx, CoordinatesExDTO>();
			Mapper.CreateMap<CoordinatesExDTO, CoordinatesEx>();
			Mapper.CreateMap<FeedbackDTO, Feedback>();
			Mapper.CreateMap<Feedback, FeedbackDTO>();
			Mapper.CreateMap<OrderEx, OrderExDTO>()
				.ForMember(o => o.FullAddressFrom, m => m.ResolveUsing<AddressFromResolver>().FromMember(t => t.AddressFrom))
				.ForMember(o => o.FullAddressTo, m => m.ResolveUsing<AddressesToResolver>().FromMember(t => t.AddressesTo));
			Mapper.CreateMap<OrderExDTO, OrderEx>();
			Mapper.CreateMap<AdditionallyRequirements, AdditionallyRequirementsDTO>();
			Mapper.CreateMap<AdditionallyRequirementsDTO, AdditionallyRequirements>();
			Mapper.CreateMap<AddressFrom, AddressFromDTO>();
			Mapper.CreateMap<AddressFromDTO, AddressFrom>();
			Mapper.CreateMap<AddressTo, AddressToDTO>();
			Mapper.CreateMap<AddressToDTO, AddressTo>();
			Mapper.CreateMap<Tarif, TarifDTO>();
			Mapper.CreateMap<TarifDTO, Tarif>();
			Mapper.CreateMap<RegistrationModelDTO, UserDTO>();
		}
	}

	class AddressFromResolver : ValueResolver<AddressFrom, string>
	{
		protected override string ResolveCore(AddressFrom source)
		{
			if (source != null)
			{
				var address = source.Address;
				if (source.Building != null)
					address += ", " + source.Building;
				return address;
			}
			return "";
		}
	}

	class AddressesToResolver : ValueResolver<List<AddressTo>, string>
	{
		protected override string ResolveCore(List<AddressTo> source)
		{
			if (source != null)
			{
				var address = "";
				foreach (var place in source)
					if (place.Address != null)
					{
						if (address != "")
							address += "; ";
						address += place.Address;
						if (place.Building != null)
							address += ", " + place.Building;
					}
					else
					{
						if (address != "")
							address += "; ";
						if (place.Building != null)
							address += place.Building;
					}
				return address;
			}
			return "";
		}
	}
}
