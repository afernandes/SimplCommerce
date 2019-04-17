using Microsoft.EntityFrameworkCore;

namespace SimplCommerce.Module.Core.Data.EntityFrameworkCore
{
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : DbContext
    {
        TDbContext GetDbContext();

        //TDbContext GetDbContext(MultiTenancySides? multiTenancySide);
    }
}
