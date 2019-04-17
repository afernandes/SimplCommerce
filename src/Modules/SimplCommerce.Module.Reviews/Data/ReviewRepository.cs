using SimplCommerce.Module.Core.Data;
using SimplCommerce.Module.Core.Data.EntityFrameworkCore;
using SimplCommerce.Module.Core.Models;
using SimplCommerce.Module.Reviews.Models;
using System.Linq;

namespace SimplCommerce.Module.Reviews.Data
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(IDbContextProvider<SimplDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public IQueryable<ReviewListItemDto> List()
        {
            var items = Table.Join(Context.Set<Entity>(),
                r => new { key1 = r.EntityId, key2 = r.EntityTypeId },
                u => new { key1 = u.EntityId, key2 = u.EntityTypeId },
                (r, u) => new ReviewListItemDto
                {
                    EntityTypeId = r.EntityTypeId,
                    Id = r.Id,
                    ReviewerName = r.ReviewerName,
                    Rating = r.Rating,
                    Title = r.Title,
                    Comment = r.Comment,
                    Status = r.Status,
                    CreatedOn = r.CreatedOn,
                    EntityName = u.Name,
                    EntitySlug = u.Slug
                });

            return items;
        }
    }
}
