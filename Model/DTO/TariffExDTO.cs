using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class TariffExDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal PriceInCity { get; set; }
		public decimal PriceOutCity { get; set; }
		public decimal PricePreOrder { get; set; }
		public decimal PriceRegularCar { get; set; }
		public decimal PriceMinivanCar { get; set; }
		public decimal PriceLuxCar { get; set; }
		public decimal PriceCourierOption { get; set; }
		public decimal PricePlateOption { get; set; }
		public decimal PriceClientCarOption { get; set; }
		public decimal PriceSpeakEnglishOption { get; set; }
		public decimal PricePassengerSmokerOption { get; set; }
		public TariffExStatus Status { get; set; }
	}
}
