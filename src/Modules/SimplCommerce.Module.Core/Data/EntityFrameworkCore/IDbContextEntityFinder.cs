using System;
using System.Collections.Generic;
using SimplCommerce.Infrastructure.Domain.Entities;

namespace SimplCommerce.Module.Core.Data.EntityFrameworkCore
{
    public interface IDbContextEntityFinder
    {
        IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType);
    }
}
