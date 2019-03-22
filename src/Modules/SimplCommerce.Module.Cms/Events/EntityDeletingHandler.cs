using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SimplCommerce.Infrastructure.Data;
using SimplCommerce.Module.Cms.Models;
using SimplCommerce.Module.Core.Events;
using SimplCommerce.Module.Core.Models;

namespace SimplCommerce.Module.Cms.Events
{
    public class EntityDeletingHandler : INotificationHandler<EntityDeletedEvent<Entity>>
    {
        private readonly IRepository<MenuItem> _menuItemRepository;

        public EntityDeletingHandler(IRepository<MenuItem> menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        /*public Task Handle(EntityDeleting notification, CancellationToken cancellationToken)
        {
            var items = _menuItemRepository.Query().Where(x => x.EntityId == notification.EntityId).ToList();
            foreach(var item in items)
            {
                _menuItemRepository.Remove(item);
            }

            return Task.CompletedTask;
        }*/

        public Task Handle(EntityDeletedEvent<Entity> notification, CancellationToken cancellationToken)
        {
            var items = _menuItemRepository.Query().Where(x => x.EntityId == notification.Entity.Id).ToList();
            foreach (var item in items)
            {
                _menuItemRepository.Remove(item);
            }

            return Task.CompletedTask;
        }
    }
}
