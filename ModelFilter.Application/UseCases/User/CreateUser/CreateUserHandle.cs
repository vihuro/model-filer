using AutoMapper;
using MediatR;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Models;

namespace ModelFilter.Application.UseCases.User.CreateUser
{
    public class CreateUserHandle : DefaultHandle, IRequestHandler<CreateUserRequest, ReturnDefault<UserReturnDefault>>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserHandle(IMediator mediator,
                                IUnitOfWork unitOfWork,
                                IUserRepository userRepository,
                                ICustomNotification notification,
                                IMapper mapper) : base(mediator, unitOfWork, notification, mapper)
        {
            _userRepository = userRepository;
        }

        public async Task<ReturnDefault<UserReturnDefault>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<UserModel>(request);


            EntityIsValid(entity);

            if (!OperationIsValid()) return null;

            _userRepository.Insert(entity);

            await Commit(cancellationToken);

            var response = new ReturnDefault<UserReturnDefault>
            {
                CurrentPage = 1,
                DataResult =
                [
                    new()
                    {
                        Id = entity.Id,
                        DateCreated = entity.DateCreated,
                        DateUpdated = entity.DateUpdated,
                        UserName = entity.UserName
                    }
                ],
                Sucess = true,
                TotalItems = 1,
                TotalPages = 1,
                MaxPerPage = 1,
            };

            return response;
        }
    }
}
