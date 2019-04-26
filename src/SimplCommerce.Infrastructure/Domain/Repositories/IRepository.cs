using SimplCommerce.Infrastructure.Models;

namespace SimplCommerce.Domain.Repositories
{
    public interface IRepository<TEntity> : IRepository<TEntity, long> where TEntity : class, IEntity<long>
    {
    }
}
