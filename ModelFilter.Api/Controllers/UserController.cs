﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModelFilter.Api.Utils;
using ModelFilter.Api.Utils.Authorize;
using ModelFilter.Application.UseCases.User;
using ModelFilter.Application.UseCases.User.CreateUser;
using ModelFilter.Application.UseCases.User.GetUser;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Models;

namespace ModelFilter.Api.Controllers
{
    /// <summary>
    /// Controller of the Users
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : BaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        public UserController(IMediator mediator, ICustomNotification notification) : base(mediator, notification)
        {
        }

        /// <summary>
        /// Get all Users
        /// </summary>
        /// <param name="filters">
        /// A object json. Example: 
        /// {
        ///     "currentPage": 1,
        ///     "maxPerPage": 100,
        ///     "filters": [
        ///         {
        ///             "field": "name",
        ///             "value": "John Doe",
        ///             "operation": "equals"
        ///         }
        ///     ],
        ///     "multiSort": [
        ///         {
        ///             "field": "dateCreated",
        ///             "value": "asc"
        ///         }
        ///     ]
        /// }
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ClaimsAuthorize("storage.sgb.write, storage.sgb.read")]
        public async Task<ActionResult<ReturnDefault<UserReturnDefault>>> GetAllUsers(string? filters, CancellationToken cancellationToken)
        {
            var filterDefault = ConvertFilter.ConvertFilterDefault(filters);

            return await CustomResponse(new GetUserRequest(filterDefault), new UserReturnDefault(), cancellationToken);
        }
        /// <summary>
        /// Create a user!
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ReturnDefault<UserReturnDefault>>> CreateUser([FromBody] CreateUserRequest request,
                                                                                     CancellationToken cancellationToken)
        {
            return await CustomCreateResponse(request, cancellationToken);
        }
    }
}
