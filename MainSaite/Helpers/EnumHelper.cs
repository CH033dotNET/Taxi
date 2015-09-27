using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Enum;
using System.Threading;
using Common.Enum.CarEnums;

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
		public static Dictionary<int, string> GetCarClasses()
		{
			var result = new Dictionary<int, string>();
			foreach (var classValue in Enum.GetValues(typeof(CarClassEnum)))
			{
				string className = "";
				switch ((CarClassEnum)classValue)
				{
					case CarClassEnum.Econom:
						className = Resources.Resource.CarClassEconom;
						break;
					case CarClassEnum.General:
						className = Resources.Resource.CarClassGeneral;
						break;
					case CarClassEnum.Premium:
						className = Resources.Resource.CarClassPremium;
						break;
				}
				result.Add((int)classValue, className);
			}
			return result;
		}
		public static Dictionary<int, string> GetCarStates()
		{
			var result = new Dictionary<int, string>();
			foreach (var stateValue in Enum.GetValues(typeof(CarStateEnum)))
			{
				string stateName = "";
				switch ((CarStateEnum)stateValue)
				{
					case CarStateEnum.Repairing:
						stateName = Resources.Resource.CarStateRepairing;
						break;
					case CarStateEnum.Working:
						stateName = Resources.Resource.CarStateWorking;
						break;
				}
				result.Add((int)stateValue, stateName);
			}
			return result;
		}
		public static Dictionary<int, string> GetCarPetrol()
		{
			var result = new Dictionary<int, string>();
			foreach (var petrolValue in Enum.GetValues(typeof(CarPetrolEnum)))
			{
				string petrolName = "";
				petrolName = EnumHelper.GetStringifyPetrol((CarPetrolEnum)petrolValue);
				result.Add((int)petrolValue, petrolName);
			}
			return result;
		}

		public static string GetStringifyPetrol(CarPetrolEnum petrol)
		{
			switch (petrol)
			{
				case CarPetrolEnum.Normal80:
					return Resources.Resource.CarPetrolNormal;
				case CarPetrolEnum.Regular92:
					return Resources.Resource.CarPetrolRegular;
				case CarPetrolEnum.Premium95:
					return Resources.Resource.CarPetrolPremium;
				case CarPetrolEnum.Super98:
					return Resources.Resource.CarPetrolSuper;
				case CarPetrolEnum.Diesel:
					return Resources.Resource.CarPetrolDiesel;
				case CarPetrolEnum.Other:
					return Resources.Resource.CarPetrolOther;
				default:
					return "";
			}
		}
		public static string GetStringifyClass(CarClassEnum carClass)
		{
			switch (carClass)
			{
				case CarClassEnum.Econom:
					return Resources.Resource.CarClassEconom;
				case CarClassEnum.General:
					return Resources.Resource.CarClassGeneral;
				case CarClassEnum.Premium:
					return Resources.Resource.CarClassPremium;
				default:
					return "";
			}
		}

		public static string GetStringifyState(CarStateEnum carState)
		{
			switch (carState)
			{
				case CarStateEnum.Repairing:
					return Resources.Resource.CarStateRepairing;
				case CarStateEnum.Working:
					return Resources.Resource.CarStateWorking;
				default:
					return "";
			}
		}
	}
}