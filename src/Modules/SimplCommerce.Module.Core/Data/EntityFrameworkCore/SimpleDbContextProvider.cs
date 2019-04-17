using Microsoft.EntityFrameworkCore;

namespace SimplCommerce.Module.Core.Data.EntityFrameworkCore
{
    public sealed class SimpleDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext
    {
        public TDbContext DbContext { get; }

        public SimpleDbContextProvider(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TDbContext GetDbContext()
        {
            return DbContext;
        }

        /*public TDbContext GetDbContext(MultiTenancySides? multiTenancySide)
        {
            return DbContext;
        }*/
    }
}
