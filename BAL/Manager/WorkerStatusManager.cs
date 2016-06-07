using BAL.Interfaces;
using Common.Enum;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DB;
using Common.Enum.DriverEnum;
using Model.DTO;
using AutoMapper;
using Model;

namespace BAL.Manager
{

	public class WorkerStatusManager : BaseManager, IWorkerStatusManager {
		public WorkerStatusManager(IUnitOfWork uOW) : base(uOW) { }

		public WorkerStatusDTO GetStatus(UserDTO Driver) {
			var driver = Mapper.Map<User>(Driver);			
			var driverStatus = uOW.WorkerStatusRepo.All.Where(o => o.WorkerId == Driver.Id).FirstOrDefault();

			return Mapper.Map<WorkerStatusDTO>(driverStatus);
		}

		public WorkerStatusDTO ChangeStatus(UserDTO Driver, DriverWorkingStatusEnum newStatus) {
			var driver = Mapper.Map<User>(Driver);
			var driverStatus = uOW.WorkerStatusRepo.All.Where(o => o.WorkerId == Driver.Id).FirstOrDefault();
			
			if (driverStatus != null) {
				driverStatus.WorkingStatus = newStatus;
				uOW.WorkerStatusRepo.Update(driverStatus);
			} else {
				driverStatus = new WorkerStatus();
				driverStatus.WorkerId = Driver.Id;
				driverStatus.WorkingStatus = newStatus;
				uOW.WorkerStatusRepo.Insert(driverStatus);
			}
			
			uOW.Save();
			return Mapper.Map<WorkerStatusDTO>(driverStatus);
		}
	}
}
