using Microsoft.EntityFrameworkCore;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Models;
using ModelFilter.Persistence.Context;

namespace ModelFilter.Persistence.Repository
{
    public class UserRepository : BaseRepository<UserModel>, IUserRepository
    {
        public UserRepository(AppDbContext appDbContext,
                             IFilterDynamic filterDynamic) : base(appDbContext, filterDynamic)
        { }

        public async Task<UserModel> GetByUserName(string userName, CancellationToken cancellationToken)
        {
            return await appDbContext.Users
                                .Where(x => 
                                x.UserName.ToLower() == userName.ToLower())
                                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
