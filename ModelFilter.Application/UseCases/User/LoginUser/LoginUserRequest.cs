using MediatR;
using ModelFilter.Domain.Models;

namespace ModelFilter.Application.UseCases.User.LoginUser
{
    public sealed record class LoginUserRequest(string UserName, string Password) : IRequest<ReturnDefault<LoginUserResponse>>;
}
