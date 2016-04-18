using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enum
{
    public enum AvailableRoles
    {
		Driver			= 1,
 		FreeDriver      = 2,
		Operator		= 3, 
		Client			= 4,
		ReportViewer	= 5, 
		Administrator	= 6
    }
	public enum AvailableRolesViaRegistration
	{
		FreeDriver = 2,
		Client = 4
	}
}
