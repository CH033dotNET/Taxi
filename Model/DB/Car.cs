using Common.Enum.CarEnums;
using Common.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
	public class Car
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(50, MinimumLength = 2)]
		public string CarName { get; set; }
		[Required]
		[StringLength(12, MinimumLength = 3)]
		public string CarNumber { get; set; }
		[Required]
		[Range(2, 20)]
		public int CarOccupation { get; set; }
		[Required]
		public CarClassEnum CarClass { get; set; }
		public string CarClassDescription
		{
			get
			{
				switch (CarClass)
				{
					case CarClassEnum.Econom:
						return Resources.Resource.CarClassEconom;
					case CarClassEnum.General:
						return Resources.Resource.CarClassGeneral;
					case CarClassEnum.Premium:
						return Resources.Resource.CarClassPremium;
					default:
						return CarClass.ToString();
				}
			}
		}
		[Required]
		public CarPetrolEnum CarPetrolType { get; set; }
		public string CarPetrolTypeDescription
		{
			get
			{
				switch (CarPetrolType)
				{
					case CarPetrolEnum.Diesel:
						return Resources.Resource.CarPetrolDiesel;
					case CarPetrolEnum.Normal80:
						return Resources.Resource.CarPetrolNormal;
					case CarPetrolEnum.Other:
						return Resources.Resource.CarPetrolOther;
					case CarPetrolEnum.Premium95:
						return Resources.Resource.CarPetrolPremium;
					case CarPetrolEnum.Regular92:
						return Resources.Resource.CarPetrolRegular;
					case CarPetrolEnum.Super98:
						return Resources.Resource.CarPetrolSuper;
					default:
						return CarPetrolType.ToString();
				}
			}
		}
		[Required]
		[Range(1, 100)]
		public int CarPetrolConsumption { get; set; }
		[Required]
		[DataType(DataType.Date)]
		[JsonConverter(typeof(CustomDateTimeConverter))]
		[DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
		public DateTime CarManufactureDate { get; set; }
		[Required]
		public CarStateEnum CarState { get; set; }
		public string CarStateDescription
		{
			get
			{
				switch (CarState)
				{
					case CarStateEnum.Working:
						return Resources.Resource.CarStateWorking;
					case CarStateEnum.Repairing:
						return Resources.Resource.CarStateRepairing;
					default:
						return CarState.ToString();
				}
			}
		}
		[Required]
		[StringLength(16, MinimumLength = 2)]
		public string CarNickName { get; set; }

		[ForeignKey("UserId")]
		[InverseProperty("CarsUser")]
		public virtual User User { get; set; }
		public int UserId { get; set; }

		[ForeignKey("UserId")]
		[InverseProperty("CarOwner")]
		public virtual User Owner { get; set; }
		public int OwnerId { get; set; }
	}
}
