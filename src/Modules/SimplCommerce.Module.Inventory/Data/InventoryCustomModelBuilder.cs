using Microsoft.EntityFrameworkCore;
using SimplCommerce.Domain.Repositories;
using SimplCommerce.Module.Inventory.Models;
using SimplCommerce.Infrastructure.Data;

namespace SimplCommerce.Module.Inventory.Data
{
    public class InventoryCustomModelBuilder : ICustomModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Warehouse>().HasData(new Warehouse(1) { Name = "Default warehouse", AddressId = 1 });
        }
    }
}
