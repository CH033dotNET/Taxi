using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enum.CarEnums
{
	public enum CarClassEnum
	{
		[Description("Premium class")]
		Premium,
		[Description("General class")]
		General,
		[Description("Econom class")]
		Econom
	}
}
