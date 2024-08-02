using Application.Clients.Dtos;
using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;
using FluentValidation;

namespace Application.Common.Validators
{
    public class WorkingContractValidator : AbstractValidator<ClientWorkingContractDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public WorkingContractValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;

            RuleFor(wc => wc.OrganizationId).ValidateWorkingContractOrganizationId(_resourceMessageProvider);
            RuleFor(wc => wc.ContractType).ValidateWorkingContractContractType(_resourceMessageProvider);
            RuleFor(wc => wc.Function).ValidateWorkingContractFunction(_resourceMessageProvider);
            RuleFor(wc => wc.FromDate).ValidateWorkingContractFromDate(_resourceMessageProvider, workingContract => workingContract.FromDate);
            RuleFor(wc => wc.ToDate).ValidateWorkingContractToDate(_resourceMessageProvider, workingContract => workingContract.FromDate);
        }
    }
}
