using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.DriversLicences.Queries;
using FluentValidation;

namespace Application.Common.Validators
{
    public class DriversLicencesValidator : AbstractValidator<DriversLicenceDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public DriversLicencesValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;
        }
    }
}
