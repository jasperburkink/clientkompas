using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Clients.Queries.GetClients;
using AutoMapper;
using Domain.CVS.Domain;
using MediatR;

namespace Application.Clients.Commands.DeactiverenClient
{
    public record DeactiverenClientCommand : IRequest<ClientDto>
        {
            public int Id { get; init; }
        }

        public class DeactiverenClientCommandHandler : IRequestHandler<DeactiverenClientCommand, ClientDto>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public DeactiverenClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ClientDto> Handle(DeactiverenClientCommand request, CancellationToken cancellationToken)
            {
                var client = await _unitOfWork.ClientRepository.GetByIDAsync(request.Id, cancellationToken);
                if (client == null)
                {
                    throw new NotFoundException(nameof(Client), request.Id);
                }

         
               // client.Diagnoses = request.Diagnoses;

                await _unitOfWork.SaveAsync(cancellationToken);

                return _mapper.Map<ClientDto>(client);
            }
        }
    }
