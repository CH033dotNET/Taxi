using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enum.DriverEnum;

namespace BAL.Interfaces
{
	public interface IWorkerStatusManager
	{
		WorkerStatusDTO GetStatus(UserDTO Driver);

		bool ChangeStatus(UserDTO Driver, DriverWorkingStatusEnum newStatus);
	}
}
