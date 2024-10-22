using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModelFilter.Application.UseCases.User;
using ModelFilter.Domain.Models;

namespace ModelFilter.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<ActionResult> CustomResponse<T>(T mediatorRequest,
                                                             CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(mediatorRequest, cancellationToken);

            return Ok(response);
        }
        protected async Task<ActionResult> CustomCreateResponse<T>(T mediatorRequest,
                                                     CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(mediatorRequest, cancellationToken);

            return Created("", response);
        }
        protected ActionResult CustomResponseError()
        {
            return BadRequest();
        }
        protected ActionResult CustomReponseError(string error)
        {
            return BadRequest(new ReturnDefault<UserReturnDefault>()
            {
                Sucess = false,
                Erros = new List<string> { error }
            });
        }
    }
}
