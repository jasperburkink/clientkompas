using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Clients.Commands.CreateClient
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateClientCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(c => c.FirstName)
            .NotEmpty().WithMessage($"{nameof(Client.FirstName)} is required.")
            .MaximumLength(50).WithMessage($"{nameof(Client.FirstName)} must not exceed 50 characters.");
        }
    }
}
