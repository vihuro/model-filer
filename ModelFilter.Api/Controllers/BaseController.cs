﻿using MediatR;
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

        protected async Task<ActionResult> CustomResponse<TRequest, TResponse>(TRequest mediatorRequest,
                                                                               TResponse responseType,
                                                                               CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(mediatorRequest, cancellationToken);

            if (_notification.HaveNotification())
                return CustomResponseError<TResponse>();

            return ValidationStatusCodeRequest<TResponse>(response);
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
                Errors = _notification.GetNotifications().Select(x => x.Message).ToList()
            });
        }
        protected ActionResult CustomResponseError(string error)
        {
            return BadRequest(new ReturnDefault<UserReturnDefault>()
            {
                Sucess = false,
                Errors = new List<string> { error }
            });
        }
        private ActionResult ValidationStatusCodeRequest<TResponse>(object? response)
        {

            if (response == null)
                return BadRequest(response);

            var dataResultPropery = response.GetType().GetProperty(nameof(ReturnDefault<TResponse>.DataResult));

            if (dataResultPropery.PropertyType == null)
                return BadRequest(response);

            var dataResultValue = dataResultPropery.GetValue(response) as List<TResponse>;

            if (dataResultValue.Count == 0)
                return NotFound(new ReturnDefault<TResponse>
                {
                    Sucess = false,
                    CurrentPage = 0,
                    DataResult = [],
                    MaxPerPage = 0,
                    TotalItems = 0,
                    TotalPages = 0,
                    Errors = ["Items not found"]
                });

            return Ok(response);
        }
    }
}
