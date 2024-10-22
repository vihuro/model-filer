using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModelFilter.Api.Utils;
using ModelFilter.Application.UseCases.User;
using ModelFilter.Application.UseCases.User.GetUser;
using ModelFilter.Domain.Models;

namespace ModelFilter.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : BaseController
    {
        public UserController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<ReturnDefault<UserReturnDefault>>> GetAllUsers(string? filters, CancellationToken cancellationToken)
        {
            try
            {
                var filterDefault = ConvertFilter.ConvertFilterDefault(filters);
                return await CustomResponse(new GetUserRequest(filterDefault), cancellationToken);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
