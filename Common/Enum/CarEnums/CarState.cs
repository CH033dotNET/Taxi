using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enum.CarEnums
{
	public enum CarStateEnum
	{
		[Description("Working state")]
		Working,
		[Description("On repair")]
		Repairing
	}
}
