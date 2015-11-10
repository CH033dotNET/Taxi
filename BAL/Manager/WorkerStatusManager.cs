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

namespace BAL.Manager
{

    public delegate void StatusChange(WorkerStatusDTO status);
	public class WorkerStatusManager : BaseManager, IWorkerStatusManager
	{
        public event StatusChange ChangeStatus;
		public WorkerStatusManager(IUnitOfWork uOW) 
			: base(uOW)
		{

		}

		/// <summary>
		/// Creates or changes status for a driver with specific id
		/// </summary>
		/// <param name="workerid">driver`s id</param>
		/// <param name="status">string representation of status used for status option choosing</param>
		public void ChangeWorkerStatus(int workerid, string status)
		{
			var isWorkerWithStatus = uOW.WorkerStatusRepo.Get(x => x.WorkerId == workerid).FirstOrDefault();
			if (isWorkerWithStatus == null)
			{
				//create new status
                isWorkerWithStatus = CreateWorkersStatus(workerid, status);
                InsertStatus(isWorkerWithStatus);
			}
			else
			{
				// edit current status
                isWorkerWithStatus = EditCurrentStatus(isWorkerWithStatus, status);
			}
            if (ChangeStatus != null) ChangeStatus(Mapper.Map<WorkerStatusDTO>(isWorkerWithStatus));

		}

		/// <summary>
		/// Shows status of specific driver using  driver`s id
		/// </summary>
		/// <param name="workerid">driver`s id</param>
		/// <returns></returns>
		public WorkerStatusDTO ShowStatus(int workerid)
		{
			var isWorkerWithStatus = uOW.WorkerStatusRepo.Get(x => x.WorkerId == workerid).FirstOrDefault();
			if (isWorkerWithStatus == null)
			{
				//create default status and shows it
				var newWorkerStatus = CreateWorkersStatus(workerid, "0");
				InsertStatus(newWorkerStatus);
				return Mapper.Map<WorkerStatusDTO>(newWorkerStatus);
			}
			else
			{
				// shows status if it exists
				return Mapper.Map<WorkerStatusDTO>(isWorkerWithStatus);
			}
		}

		/// <summary>
		/// Creates status for a driver with specific id
		/// </summary>
		/// <param name="workerid">driver`s id</param>
		/// <param name="status">string representation of status used for status option choosing</param>
		/// <returns></returns>
		private WorkerStatus CreateWorkersStatus(int workerid, string status)
		{
			var newStatus = new WorkerStatus();
			newStatus.WorkingStatus = ChooseStatusOption(status);
			newStatus.WorkerId = workerid;
			return newStatus;
		}

		/// <summary>
		/// Wrapper for option choosing
		/// </summary>
		/// <param name="option">string representation of status used for status option choosing</param>
		/// <returns></returns>
		private DriverWorkingStatusEnum ChooseStatusOption(string option)
		{
			switch (option)
			{
				case "0":
					return DriverWorkingStatusEnum.NoStatus;
				case "1":
					return DriverWorkingStatusEnum.HasOrder;
				case "2":
					return DriverWorkingStatusEnum.DoingOrder;
				case "3":
					return DriverWorkingStatusEnum.AwaitingOrder;
				case "4":
					return DriverWorkingStatusEnum.Resting;
				default:
					return DriverWorkingStatusEnum.NoStatus;
			}
		}

		/// <summary>
		/// Inserts status object into the database
		/// </summary>
		/// <param name="status">object you wish to insert</param>
		private void InsertStatus(WorkerStatus status)
		{
			uOW.WorkerStatusRepo.Insert(status);
			uOW.Save();
		}
		/// <summary>
		/// Changes status for a driver with specific id
		/// </summary>
		/// <param name="oldStatus">object you wish to change</param>
		/// <param name="status">string representation of status used for status option choosing</param>
		/// <returns></returns>
		private WorkerStatus EditCurrentStatus(WorkerStatus oldStatus, string status)
		{
			uOW.WorkerStatusRepo.SetStateModified(oldStatus);
			oldStatus.WorkingStatus = ChooseStatusOption(status);
			uOW.Save();
			return oldStatus;
		}
	}
}
