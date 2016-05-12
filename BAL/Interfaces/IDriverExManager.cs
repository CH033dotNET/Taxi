using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DTO;

namespace BAL.Interfaces
{
	public interface IDriverExManager
	{
		void AddDriverLocation(CoordinatesExDTO coordinate);
	}
}
