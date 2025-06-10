using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.DriversLicences.Queries;

namespace Application.Common.Validators
{
    public class DriversLicencesValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider) : AbstractValidator<DriversLicenceDto>
    {
    }
}
