using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Model;
using Model.DTO;

namespace Common
{
	public class AutoMapperConfig
	{
		public static void Configure()
		{
			Mapper.CreateMap<User, UserDTO>();
		}
	}
}
