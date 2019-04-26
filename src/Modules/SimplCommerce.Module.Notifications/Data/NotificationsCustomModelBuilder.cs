using Microsoft.EntityFrameworkCore;
using SimplCommerce.Domain.Repositories;
using SimplCommerce.Module.Notifications.Models;
using SimplCommerce.Infrastructure.Data;

namespace SimplCommerce.Module.Notifications.Data
{
    public class NotificationsCustomModelBuilder : ICustomModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserNotification>().ToTable("Notifications_UserNotification");
        }
    }
}
