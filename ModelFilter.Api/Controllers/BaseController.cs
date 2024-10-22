using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        protected ActionResult CustomReponseError()
        {
            return BadRequest();
        }
        protected ActionResult CustomReponseError(string error)
        {
            return BadRequest(error);
        }
    }
}
