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
				RuleFor(district => district.Id).Equal(0).WithMessage("Incorrect Id");
				RuleFor(district => district.Name).NotEmpty()
					.Matches(@"([A-Z]{0,1}[a-z]{1,10}\s{0,1}){1,4}")
					.Length(4, 30)
					.WithLocalizedMessage(() => Resources.Resource.DistrictNameError);
			});

			RuleSet("EditDistrict", () => {
				RuleFor(district => district.Name).NotEmpty().Matches(@"([A-Z]{0,1}[a-z]{1,10}\s{0,1}){1,4}").Length(4, 30).WithLocalizedMessage(() => Resources.Resource.DistrictNameError);
			});
		}
	}
}
