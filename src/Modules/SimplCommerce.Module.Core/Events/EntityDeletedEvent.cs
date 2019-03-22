using MediatR;
using SimplCommerce.Infrastructure.Models;

namespace SimplCommerce.Module.Core.Events
{
    /*public class EntityDeleting : INotification
    {
        public long EntityId { get; set; }
    }*/

    /// <summary>
    /// A container for passing entities that have been deleted. This is not used for entities that are deleted logically via a bit column.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityDeletedEvent<T> : INotification where T : EntityBase
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="entity">Entity</param>
        public EntityDeletedEvent(T entity)
        {
            Entity = entity;
        }

        /// <summary>
        /// Entity
        /// </summary>
        public T Entity { get; }
    }

}
