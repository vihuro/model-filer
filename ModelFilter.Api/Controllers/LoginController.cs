using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModelFilter.Application.UseCases.User.LoginUser;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Models;

namespace ModelFilter.Api.Controllers
{
    [ApiController]
    [Route("api/v1/login")]
    public class LoginController : BaseController
    {
        public LoginController(IMediator mediator, ICustomNotification notification) : base(mediator, notification)
        {
        }
        [HttpPost]
        public async Task<ActionResult<string>> Login(LoginUserRequest request, CancellationToken cancellationToken)
        {
            return await CustomResponse(request,new LoginUserResponse(), cancellationToken);
        }
    }
}
