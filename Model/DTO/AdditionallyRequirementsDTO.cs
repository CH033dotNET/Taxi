using Common.Enum.CarEnums;
using Model.DB;
using System;

namespace Model.DTO
{
	public class AdditionallyRequirementsDTO
	{
		public int Id { get; set; }

		public bool Urgently { get; set; }

		public string Time { get; set; }

		public int Passengers { get; set; }

		public CarClassEnum Car { get; set; }

		public bool Courier { get; set; }

		public bool WithPlate { get; set; }

		public bool MyCar { get; set; }

		public bool Pets { get; set; }

		public bool Bag { get; set; }

		public bool Conditioner { get; set; }

		public bool NoSmoking { get; set; }

		public bool Smoking { get; set; }

		public bool English { get; set; }

		public bool Check { get; set; }
	}
}
