using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SimplCommerce.Domain.Repositories;
using SimplCommerce.Module.Cms.Models;
using SimplCommerce.Module.Core.Events;

namespace SimplCommerce.Module.Cms.Events
{
    public class EntityDeletingHandler : INotificationHandler<EntityDeleting>
    {
        private readonly IRepository<MenuItem> _menuItemRepository;

        public EntityDeletingHandler(IRepository<MenuItem> menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public Task Handle(EntityDeleting notification, CancellationToken cancellationToken)
        {
            var items = _menuItemRepository.GetAll().Where(x => x.EntityId == notification.EntityId).ToList();
            foreach(var item in items)
            {
                _menuItemRepository.Delete(item);
            }

            return Task.CompletedTask;
        }
    }
}
