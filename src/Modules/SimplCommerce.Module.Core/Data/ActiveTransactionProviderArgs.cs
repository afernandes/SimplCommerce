using System.Collections.Generic;

namespace SimplCommerce.Module.Core.Data
{
    public class ActiveTransactionProviderArgs : Dictionary<string, object>
    {
        public static ActiveTransactionProviderArgs Empty { get; } = new ActiveTransactionProviderArgs();
    }
}
