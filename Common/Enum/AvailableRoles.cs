﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enum
{
    public enum AvailableRoles
    {
		Driver			= 1, 
		Operator		= 2, 
		Client			= 3,
		ReportViewer	= 4, 
		Administrator	= 5,
		Support			= 6,
		FreeDriver      = 7
	}
	public enum AvailableRolesViaRegistration
	{
		Client = 3,
		FreeDriver = 7
	}
}
