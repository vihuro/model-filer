using MediatR;
using ModelFilter.Domain.Models;
using ModelFilter.Domain.Utils.Filters;

namespace ModelFilter.Application.UseCases.User.GetUser
{
    public sealed record GetUserRequest(FilterBase Filters) : IRequest<ReturnDefault<UserReturnDefault>>;
}
