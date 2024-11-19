using ModelFilter.Domain.Models;

namespace ModelFilter.Domain.Interface
{
    public interface IUserRepository : IBaseRepository<UserModel>
    {
        Task<UserModel> GetByUserName(string userName, CancellationToken cancellationToken);
    }
}
