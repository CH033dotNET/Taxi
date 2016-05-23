using Common.Enum;
using Common.Enum.CarEnums;
using Common.Enum.DriverEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
	public static class EnumHelper
	{

		public static Dictionary<int, string> GetWorkerStatus()
		{
			var result = new Dictionary<int, string>();
			foreach (var val in System.Enum.GetValues(typeof(DriverWorkingStatusEnum)))
			{
				string name = "";
				name = EnumHelper.GetStringifyStatus((DriverWorkingStatusEnum)val);
				result.Add((int)val, name);
			}
			return result;
		}

		public static string GetStringifyStatus(DriverWorkingStatusEnum status)
		{
			switch (status)
			{
				case DriverWorkingStatusEnum.DoingOrder:
					return Resources.Resource.DoingOrder;
				case DriverWorkingStatusEnum.AwaitingOrder:
					return Resources.Resource.AwaitingOrder;
				default:
					return "";
			}
		}

		public static Dictionary<int, string> GetRoles()
		{
			var result = new Dictionary<int, string>();
			foreach (var val in System.Enum.GetValues(typeof(AvailableRoles)))
			{
				string name = "";
				switch ((AvailableRoles)val)
				{
					case AvailableRoles.Driver:
						name = Resources.Resource.Driver;
						break;
					case AvailableRoles.FreeDriver:
						name = Resources.Resource.FreeDriver;
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

				result.Add((int)val, name);
			}
			return result;
		}
		public static Dictionary<int, string> GetCarClasses()
		{
			var result = new Dictionary<int, string>();
			foreach (var classValue in System.Enum.GetValues(typeof(CarClassEnum)))
			{
				string className = "";
				switch ((CarClassEnum)classValue)
				{
					case CarClassEnum.Normal:
						className = Resources.Resource.Normal;
						break;
					case CarClassEnum.Universal:
						className = Resources.Resource.Universal;
						break;
					case CarClassEnum.Minivan:
						className = Resources.Resource.Minivan;
						break;
					case CarClassEnum.Lux:
						className = Resources.Resource.Lux;
						break;
				}
				result.Add((int)classValue, className);
			}
			return result;
		}
		public static Dictionary<int, string> GetCarStates()
		{
			var result = new Dictionary<int, string>();
			foreach (var stateValue in System.Enum.GetValues(typeof(CarStateEnum)))
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
			foreach (var petrolValue in System.Enum.GetValues(typeof(CarPetrolEnum)))
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
				case CarClassEnum.Normal:
					return Resources.Resource.Normal;
				case CarClassEnum.Universal:
					return Resources.Resource.Universal;
				case CarClassEnum.Minivan:
					return Resources.Resource.Minivan;
				case CarClassEnum.Lux:
					return Resources.Resource.Lux;
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
