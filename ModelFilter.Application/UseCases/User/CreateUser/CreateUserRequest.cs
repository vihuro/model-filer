using MediatR;
using ModelFilter.Domain.Models;

namespace ModelFilter.Application.UseCases.User.CreateUser
{
    public sealed record CreateUserRequest(string UserName, string? Name, string Password) : IRequest<ReturnDefault<UserReturnDefault>>
    {
    }
}
