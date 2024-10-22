using MediatR;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Models;

namespace ModelFilter.Application.UseCases.User.GetUser
{
    public class GetUserHandle : DefaultHandle,
        IRequestHandler<GetUserRequest, ReturnDefault<UserReturnDefault>>
    {
        private readonly IUserRepository _userRepository;
        public GetUserHandle(IMediator mediator,
                             IUnitOfWork unitOfWork,
                             IUserRepository userRepository) : base(mediator, unitOfWork)
        {
            _userRepository = userRepository;
        }

        public async Task<ReturnDefault<UserReturnDefault>> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            var userReponse = await _userRepository.GetAsync(request.Filters, 100);
            var response = new ReturnDefault<UserReturnDefault>()
            {
                CurrentPage = userReponse.CurrentPage,
                TotalItems = userReponse.TotalItems,
                MaxPerPage = userReponse.MaxPerPage,
                TotalPages = userReponse.TotalPages,
                DataResult = userReponse.DataResult.Select(x => new UserReturnDefault
                {
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    Id = x.Id,
                    UserName = x.UserName,
                }).ToList()
            };
            return response;
        }
    }
}
