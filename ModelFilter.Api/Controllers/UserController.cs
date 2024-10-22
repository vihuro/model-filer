using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModelFilter.Api.Utils;
using ModelFilter.Application.UseCases.User;
using ModelFilter.Application.UseCases.User.CreateUser;
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

        /// <summary>
        /// Buscar todos usuários!
        /// </summary>
        /// <param name="filters">
        /// Um objeto JSON que representa os filtros. Exemplo:
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
        public async Task<ActionResult<ReturnDefault<UserReturnDefault>>> GetAllUsers(string? filters, CancellationToken cancellationToken)
        {
            try
            {
                var filterDefault = ConvertFilter.ConvertFilterDefault(filters);
                return await CustomResponse(new GetUserRequest(filterDefault), cancellationToken);

            }
            catch (Exception ex)
            {

                return CustomReponseError(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<ReturnDefault<UserReturnDefault>>> CreateUser([FromBody] CreateUserRequest request, 
                                                                                     CancellationToken cancellationToken)
        {
            try
            {
                return await CustomCreateResponse(request, cancellationToken);
            }
            catch (Exception ex)
            {

                return CustomReponseError(ex.Message);
            }
        }
    }
}
