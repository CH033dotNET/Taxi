using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Model.DTO;
using DAL;

namespace BAL.Tools
{
	public class CarModelValidator : AbstractValidator<CarDTO>
	{
		public CarModelValidator()
		{
			RuleSet("AddNewCar", () => {
				RuleFor(xcar => xcar.Id).Equal(0).WithMessage("[Id is not matching validator rules]"); // validation rule for {Id} property
				RuleFor(xcar => xcar.CarName).NotEmpty().Length(1, 50).WithLocalizedMessage(() => Resources.Resource.CarNameError); // validation rule for {CarName} property
				RuleFor(xcar => xcar.CarNickName).NotEmpty().Length(2, 4).WithLocalizedMessage(() => Resources.Resource.CarNickNameError); // validation rule for {CarNickName} property
				RuleFor(xcar => xcar.CarNumber).NotEmpty().Length(3, 11).Matches(@"[A-Z]{2}\d{4}[A-Z]{2}").Must(BeUnique).WithLocalizedMessage(() => Resources.Resource.CarNumberError); // validation rule for {CarNumber} property
				RuleFor(xcar => xcar.CarOccupation).NotEmpty().LessThanOrEqualTo(20).WithLocalizedMessage(() => Resources.Resource.CarNumberError); // validation rule for {CarOccupation} property
				RuleFor(xcar => xcar.CarManufactureDate).NotEmpty().LessThan(DateTime.Now).Must(BeAValidDate).WithLocalizedMessage(() => Resources.Resource.CarMDError); // validation rule for {CarManufactureDate} property
				RuleFor(xcar => xcar.UserId).NotEmpty().Equal(xcar => xcar.OwnerId).WithMessage("[UserId is not matching OwnerId property]"); // validation rule for {UserId} property
			});

			RuleSet("EditThisCar", () =>
			{
				RuleFor(xcar => xcar.CarName).NotEmpty().Length(1, 50).WithLocalizedMessage(() => Resources.Resource.CarNameError); // validation rule for {CarName} property
				RuleFor(xcar => xcar.CarNickName).NotEmpty().Length(2, 4).WithLocalizedMessage(() => Resources.Resource.CarNickNameError); // validation rule for {CarNickName} property
				RuleFor(xcar => xcar.CarNumber).NotEmpty().Length(3, 11).Matches(@"[A-Z]{2}\d{4}[A-Z]{2}").WithLocalizedMessage(() => Resources.Resource.CarNumberError); // validation rule for {CarNumber} property
				RuleFor(xcar => xcar.CarOccupation).NotEmpty().LessThanOrEqualTo(20).WithLocalizedMessage(() => Resources.Resource.CarNumberError); // validation rule for {CarOccupation} property
				RuleFor(xcar => xcar.CarManufactureDate).NotEmpty().LessThan(DateTime.Now).Must(BeAValidDate).WithLocalizedMessage(() => Resources.Resource.CarMDError); // validation rule for {CarManufactureDate} property
				RuleFor(xcar => xcar.UserId).NotEmpty().Equal(xcar => xcar.OwnerId).WithMessage("[UserId is not matching OwnerId property]"); // validation rule for {UserId} property
			});
		}

		private bool BeAValidDate(DateTime date)
		{
			return !date.Equals(default(DateTime));
		}
		private bool BeUnique(string number)
		{
			MainContext main = new MainContext();
			var dublicate = main.Cars.Any(x => x.CarNumber == number);
			return !dublicate;
		}
	}
}
