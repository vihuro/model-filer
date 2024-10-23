using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModelFilter.Application.UseCases.User;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Models;

namespace ModelFilter.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICustomNotification _notification;

        public BaseController(IMediator mediator, ICustomNotification notification)
        {
            _mediator = mediator;
            _notification = notification;
        }

        protected async Task<ActionResult> CustomResponse<T>(T mediatorRequest,
                                                             CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(mediatorRequest, cancellationToken);
            if (_notification.HaveNotification())
                return CustomResponseError<T>();

            return Ok(response);
        }
        protected async Task<ActionResult> CustomCreateResponse<T>(T mediatorRequest,
                                                     CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(mediatorRequest, cancellationToken);
            if (_notification.HaveNotification())
                return CustomResponseError<T>();

            return Created("", response);
        }
        protected ActionResult CustomResponseError<T>()
        {
            return BadRequest(new ReturnDefault<T>
            {
                Sucess = false,
                Erros = _notification.GetNotifications().Select(x => x.Message).ToList()
            });
        }
        protected ActionResult CustomResponseError(string error)
        {
            return BadRequest(new ReturnDefault<UserReturnDefault>()
            {
                Sucess = false,
                Erros = new List<string> { error }
            });
        }
    }
}
