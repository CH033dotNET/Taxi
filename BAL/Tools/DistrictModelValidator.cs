using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Model;
using DAL;

namespace BAL.Tools
{
	public class DistrictModelValidator : AbstractValidator<District>
	{
		public DistrictModelValidator()
		{
			RuleSet("AddDistrict", () => {
				RuleFor(district => district.Id).Equal(0).WithMessage("Ololo");
				RuleFor(district => district.Name).NotEmpty().Length(4, 30).WithLocalizedMessage(() => "");
			});

			RuleSet("EditDistrict", () => {
				RuleFor(district => district.Name).NotEmpty().Length(4, 30).WithLocalizedMessage(() => "");
			});
		}
	}
}
