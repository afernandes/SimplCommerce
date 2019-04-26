using Microsoft.EntityFrameworkCore;
using SimplCommerce.Infrastructure;
using SimplCommerce.Domain.Repositories;
using SimplCommerce.Infrastructure.Localization;
using SimplCommerce.Infrastructure.Data;

namespace SimplCommerce.Module.Localization.Data
{
    public class LocalizationCustomModelBuilder : ICustomModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Culture>().HasData(
               new Culture(GlobalConfiguration.DefaultCulture) { Name = "English (US)" }
            );
            modelBuilder.Entity<Culture>().ToTable("Localization_Culture");
            modelBuilder.Entity<Resource>().ToTable("Localization_Resource");
        }
    }
}
