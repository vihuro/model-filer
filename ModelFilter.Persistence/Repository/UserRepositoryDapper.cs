using Microsoft.Extensions.Configuration;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Models;

namespace ModelFilter.Persistence.Repository
{
    public class UserRepositoryDapper : BaseRepositoryDapper<UserModel>, IUserRepositoryDapper
    {
        public UserRepositoryDapper(IConfiguration configuration) : base (configuration)
        { }

    }
}
