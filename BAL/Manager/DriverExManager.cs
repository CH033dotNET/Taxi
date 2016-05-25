using AutoMapper;
using BAL.Interfaces;
using DAL.Interface;
using Model.DB;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Manager
{
	public class DriverExManager : BaseManager, IDriverExManager
	{

		public DriverExManager(IUnitOfWork uOW) : base(uOW)
		{ }

		public void AddDriverLocation(CoordinatesExDTO coordinate)
		{
			var location = Mapper.Map<CoordinatesEx>(coordinate);
			if (location.OrderId == 0)
				location.OrderId = null;
			uOW.CoordinatesExRepo.Insert(location);
			uOW.Save();
		}
	}
}
