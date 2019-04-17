using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplCommerce.Infrastructure.Data;
using SimplCommerce.Module.Core.Models;

namespace SimplCommerce.Module.Core.Areas.Core.Controllers
{
    [Area("Core")]
    [Authorize(Roles = "admin")]
    [Route("api/entity-types")]
    public class EntityTypeApiController : Controller
    {
        private readonly IRepository<EntityType, string> _entityTypeRepository;

        public EntityTypeApiController(IRepository<EntityType, string> entityTypeRepository)
        {
            _entityTypeRepository = entityTypeRepository;
        }

        [HttpGet("menuable")]
        public IActionResult GetMenuable()
        {
            var entityTypes = _entityTypeRepository.GetAll()
                .Where(x => x.IsMenuable)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                });

            return Ok(entityTypes);
        }
    }
}
