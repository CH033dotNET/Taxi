using BAL.Interfaces;
using DAL.Interface;
using System.Collections.Generic;
using System.Linq;
using Model.DB;
using Common.Enum.DriverEnum;
using Model.DTO;
using AutoMapper;
using Model;
using System;

namespace BAL.Manager
{
	public class WorkerStatusManager : BaseManager, IWorkerStatusManager
	{
		public WorkerStatusManager(IUnitOfWork uOW) : base(uOW) { }

		public WorkerStatusDTO GetStatus(UserDTO Driver)
		{
			var driver = Mapper.Map<User>(Driver);
			var driverStatus = uOW.WorkerStatusRepo.All.Where(o => o.WorkerId == Driver.Id).FirstOrDefault();

			return Mapper.Map<WorkerStatusDTO>(driverStatus);
		}

		public List<WorkerStatusDTO> GetAllStatuses()
		{
			return Mapper.Map<List<WorkerStatusDTO>>(uOW.WorkerStatusRepo.All);
		}

		public WorkerStatusDTO ChangeStatus(UserDTO Driver, DriverWorkingStatusEnum newStatus, DateTime? blockTime = null, string message = null)
		{
			var driver = Mapper.Map<User>(Driver);
			var driverStatus = uOW.WorkerStatusRepo.All.Where(o => o.WorkerId == Driver.Id).FirstOrDefault();
			
			if (driverStatus != null)
			{
				driverStatus.WorkingStatus = newStatus;
				if (blockTime != null)
					driverStatus.BlockTime = blockTime;
				if (message != null)
					driverStatus.BlockMessage = message;
				uOW.WorkerStatusRepo.Update(driverStatus);
			}
			else
			{
				driverStatus = new WorkerStatus();
				driverStatus.WorkerId = Driver.Id;
				driverStatus.WorkingStatus = newStatus;
				if (blockTime != null)
					driverStatus.BlockTime = blockTime;
				if (message != null)
					driverStatus.BlockMessage = message;
				uOW.WorkerStatusRepo.Insert(driverStatus);
			}
			
			uOW.Save();
			return Mapper.Map<WorkerStatusDTO>(driverStatus);
		}

		public bool DeleteStatus(int driverId)
		{
			try
			{
				var status = uOW.WorkerStatusRepo.All.Where(w => w.WorkerId == driverId).FirstOrDefault();
				uOW.WorkerStatusRepo.Delete(status);
				uOW.Save();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
