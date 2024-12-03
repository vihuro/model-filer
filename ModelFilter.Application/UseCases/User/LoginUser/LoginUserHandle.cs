using AutoMapper;
using MediatR;
using ModelFilter.Application.Interface;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Models;
using static BCrypt.Net.BCrypt;

namespace ModelFilter.Application.UseCases.User.LoginUser
{
    public class LoginUserHandle : DefaultHandle,
                 IRequestHandler<LoginUserRequest, ReturnDefault<LoginUserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public LoginUserHandle(IMediator mediator,
                               IUnitOfWork unitOfWork,
                               ICustomNotification notification,
                               IMapper mapper,
                               IUserRepository userRepository,
                               IJwtService jwtService) : base(mediator, unitOfWork, notification, mapper)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<ReturnDefault<LoginUserResponse>> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserName(request.UserName, cancellationToken);


            if (user is null)
            {
                Notify("Invalid username or password, please reenter them.");
                return null;
            }
            var passwordIsValid = Verify(request.Password, user.Password);

            if (!passwordIsValid)
            {
                Notify("Invalid username or password, please reenter them.");
                return null;
            }


            var (accessToken, refreshToken) = _jwtService.Token("", user.UserName, user.Id.ToString());

            return new ReturnDefault<LoginUserResponse>
            {
                Sucess = true,

                DataResult =
                [
                    new LoginUserResponse
                    {
                        RefreshToken = refreshToken,
                        AccessToken = accessToken,
                    }
                ]
            };

        }
    }
}
