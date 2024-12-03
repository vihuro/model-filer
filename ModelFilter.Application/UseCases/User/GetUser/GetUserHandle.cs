using AutoMapper;
using MediatR;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Models;

namespace ModelFilter.Application.UseCases.User.GetUser
{
    public class GetUserHandle : DefaultHandle,
                 IRequestHandler<GetUserRequest, ReturnDefault<UserReturnDefault>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRepositoryDapper _userRepositoryDaaper;

        public GetUserHandle(IMediator mediator,
                             IUnitOfWork unitOfWork,
                             ICustomNotification notification,
                             IMapper mapper,
                             IUserRepository userRepository,
                             IUserRepositoryDapper userRepositoryDaaper) : base(mediator, unitOfWork, notification, mapper)
        {
            _userRepository = userRepository;
            _userRepositoryDaaper = userRepositoryDaaper;
        }

        public async Task<ReturnDefault<UserReturnDefault>> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            //var userReponse = await _userRepository.GetAsync(request.Filters, cancellationToken, 10);
            var userReponse = await _userRepositoryDaaper.GetAsync(request.Filters, cancellationToken, 10);

            var response = _mapper.Map<ReturnDefault<UserReturnDefault>>(userReponse);

            return response;
        }
    }
}
