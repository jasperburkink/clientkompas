using Application.Common.Interfaces.CVS;
using Application.DriversLicences.Queries;
using FluentValidation;

namespace Application.Common.Validators
{
    public class DriversLicencesValidator : AbstractValidator<DriversLicenceDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DriversLicencesValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
