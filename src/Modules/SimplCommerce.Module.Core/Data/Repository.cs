using Microsoft.EntityFrameworkCore;

using SimplCommerce.Infrastructure.Data;
using SimplCommerce.Infrastructure.Models;
using SimplCommerce.Module.Core.Data.EntityFrameworkCore;


namespace SimplCommerce.Module.Core.Data
{
    public class Repository<TEntity, TPrimaryKey> : Repository<SimplDbContext, TEntity, TPrimaryKey>, IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>        
    {
        public Repository(IDbContextProvider<SimplDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }


    public class Repository<TEntity> : Repository<SimplDbContext, TEntity, long>, IRepository<TEntity>
        where TEntity : class, IEntity<long>
    {
        public Repository(IDbContextProvider<SimplDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }


    public class RepositoryContext<TDbContext, TEntity> : Repository<TDbContext, TEntity, long>, IRepository<TEntity>
        where TEntity : class, IEntity<long>
        where TDbContext : DbContext
    {
        public RepositoryContext(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
