using System.Linq;
using SimplCommerce.Module.ActivityLog.Models;
using SimplCommerce.Module.Core.Data;
using SimplCommerce.Module.Core.Data.EntityFrameworkCore;
using SimplCommerce.Module.Core.Models;

namespace SimplCommerce.Module.ActivityLog.Data
{
    public class ActivityRepository : Repository<Activity>, IActivityTypeRepository
    {
        private const int MostViewActivityTypeId = 1;

        public ActivityRepository(IDbContextProvider<SimplDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public IQueryable<MostViewEntityDto> List()
        {
            return from a in Table
                join e in Context.Set<Entity>() on new { a.EntityId, a.EntityTypeId } equals new { e.EntityId, e.EntityTypeId }
                where a.ActivityTypeId == MostViewActivityTypeId
                group a by new {a.EntityId, a.EntityTypeId, e.Name, e.Slug}
                into g
                orderby g.Count() descending
                select new MostViewEntityDto
                {
                    EntityTypeId = g.Key.EntityTypeId,
                    EntityId = g.Key.EntityId,
                    Name = g.Key.Name,
                    Slug = g.Key.Slug,
                    ViewedCount = g.Count()
                };
        }
    }
}
