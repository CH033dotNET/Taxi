using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Enum;
using System.Threading;

namespace MainSaite.Helpers
{
	public static class EnumHelper
	{
		public static Dictionary<int,string> GetRoles()
		{
			var result = new Dictionary<int,string>();
			foreach(var val in Enum.GetValues(typeof(AvailableRoles)))
			{
				string name = "";
				switch((AvailableRoles)val)
				{
					case AvailableRoles.Driver:
						name = Resources.Resource.Driver;
						break;

					case AvailableRoles.Administrator:
						name = Resources.Resource.Administrator;
						break;

					case AvailableRoles.Client:
						name = Resources.Resource.Client;
						break;

					case AvailableRoles.Operator:
						name = Resources.Resource.Operator;
						break;

					case AvailableRoles.ReportViewer:
						name = Resources.Resource.ReportViewer;
						break;
				}

				result.Add((int)val,name);
			}
			return result;
		}
	}
}