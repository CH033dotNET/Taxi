using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class OrderDTO
	{
		public OrderDTO()
		{
			this.District = null;
		}

		public int Id { get; set; }

		public string PeekPlace { get; set; }

		public string DropPlace { get; set; }

		public DateTime OrderTime { get; set; }

		public string RunTime { get; set; }

		public double LatitudeDropPlace { get; set; }

		public double LongitudeDropPlace { get; set; }

		public double Accuracy { get; set; }

		public double LatitudePeekPlace { get; set; }

		public double LongitudePeekPlace { get; set; }

		public int IsConfirm { get; set; }

		public Nullable<DateTime> StartWork { get; set; }

		public Nullable<DateTime> EndWork { get; set; }

		public int DriverId { get; set; }

		public string WaitingTime { get; set; }

		public decimal TotalPrice { get; set; }

		public virtual PersonDTO Person { get; set; }

		public int PersonId { get; set; }

		public virtual District District { get; set; }

		public int? DistrictId { get; set; }

		public float FuelSpent { get; set; }

		public string FirstName { get; set; }

		public string DriverName { get; set; }

	}
}
