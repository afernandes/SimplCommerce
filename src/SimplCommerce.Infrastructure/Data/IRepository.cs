using SimplCommerce.Infrastructure.Models;

namespace SimplCommerce.Infrastructure.Data
{
    public interface IRepository<TEntity> : IRepository<TEntity, long> where TEntity : class, IEntity<long>
    {
    }
}
