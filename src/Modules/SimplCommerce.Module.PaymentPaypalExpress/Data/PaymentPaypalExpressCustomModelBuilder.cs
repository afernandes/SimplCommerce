using Microsoft.EntityFrameworkCore;
using SimplCommerce.Domain.Repositories;
using SimplCommerce.Module.Payments.Models;
using SimplCommerce.Infrastructure.Data;

namespace SimplCommerce.Module.PaymentPaypalExpress.Data
{
    public class PaymentPaypalExpressCustomModelBuilder : ICustomModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentProvider>().HasData(
                new PaymentProvider("PaypalExpress") { Name = "Paypal Express", LandingViewComponentName = "PaypalExpressLanding", ConfigureUrl = "payments-paypalExpress-config", IsEnabled = true, AdditionalSettings = "{ \"IsSandbox\":true, \"ClientId\":\"\", \"ClientSecret\":\"\" }" }
            );
        }
    }
}
