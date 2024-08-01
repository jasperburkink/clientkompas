using Application.Clients.Dtos;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;
using FluentValidation;

namespace Application.Common.Validators
{
    public class WorkingContractValidator : AbstractValidator<ClientWorkingContractDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkingContractValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(wc => wc.OrganizationId).ValidateWorkingContractOrganization();
            RuleFor(wc => wc.ContractType).ValidateWorkingContractContractType();
            RuleFor(wc => wc.Function).ValidateWorkingContractFunction();
            RuleFor(wc => wc.FromDate).ValidateWorkingContractFromDate(workingContract => workingContract.ToDate);
            RuleFor(wc => wc.ToDate).ValidateWorkingContractToDate(workingContract => workingContract.FromDate);
        }
    }
}
