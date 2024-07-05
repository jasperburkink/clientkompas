using Application.Common.Interfaces.CVS;
using Application.Diagnoses.Queries.GetDiagnosis;
using FluentValidation;

namespace Application.Common.Validators
{
    public class DiagnosisValidator : AbstractValidator<DiagnosisDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiagnosisValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
