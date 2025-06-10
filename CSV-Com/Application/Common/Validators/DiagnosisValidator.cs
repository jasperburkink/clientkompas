using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;
using Application.Diagnoses.Queries.GetDiagnosis;
using FluentValidation;

namespace Application.Common.Validators
{
    public class DiagnosisValidator : AbstractValidator<DiagnosisDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public DiagnosisValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;

            RuleFor(d => d.Name).ValidateDiagnosisName(_resourceMessageProvider);
        }
    }
}
