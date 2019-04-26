using Microsoft.EntityFrameworkCore;
using SimplCommerce.Domain.Repositories;
using SimplCommerce.Module.Payments.Models;
using SimplCommerce.Infrastructure.Data;

namespace SimplCommerce.Module.PaymentCoD.Data
{
    public class PaymentCoDCustomModelBuilder : ICustomModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentProvider>().HasData(
                new PaymentProvider("CoD") { Name = "Cash On Delivery", LandingViewComponentName = "CoDLanding", ConfigureUrl = "payments-cod-config", IsEnabled = true, AdditionalSettings = null }
            );
        }
    }
}
