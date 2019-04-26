using System;
using System.Collections.Generic;
using System.Text;

namespace SimplCommerce.Module.Core.Data.EntityFrameworkCore.Repositories
{
    public interface IEfGenericRepositoryRegistrar
    {
        void RegisterForDbContext(Type dbContextType, IIocManager iocManager, AutoRepositoryTypesAttribute defaultAutoRepositoryTypesAttribute);
    }
}
