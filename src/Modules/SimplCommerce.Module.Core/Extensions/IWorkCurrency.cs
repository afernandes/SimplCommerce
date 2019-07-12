using SimplCommerce.Module.Core.Models;

namespace SimplCommerce.Module.Core.Extensions
{
    public interface IWorkCurrency
    {
        /// <summary>
        /// Gets or sets current user working currency
        /// </summary>
        Currency WorkingCurrency { get; set; }
    }
}
