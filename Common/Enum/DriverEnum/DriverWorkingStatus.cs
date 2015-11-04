using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enum.DriverEnum
{
	public enum DriverWorkingStatusEnum
	{
		NoStatus = 0,
		HasOrder = 1,
		DoingOrder = 2,
		AwaitingOrder = 3,
		Resting = 4
	}
}
