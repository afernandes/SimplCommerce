using Microsoft.EntityFrameworkCore;
using SimplCommerce.Domain.Repositories;
using SimplCommerce.Module.ActivityLog.Models;
using SimplCommerce.Infrastructure.Data;

namespace SimplCommerce.Module.ActivityLog.Data
{
    public class ActivityLogCustomModelBuilder : ICustomModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>().HasIndex(x => x.ActivityTypeId);

            modelBuilder.Entity<ActivityType>().HasData(new ActivityType(1) { Name = "EntityView" });
        }
    }
}
