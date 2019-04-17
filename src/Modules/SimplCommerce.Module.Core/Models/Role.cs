using Microsoft.AspNetCore.Identity;
using SimplCommerce.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SimplCommerce.Module.Core.Models
{
    public class Role : IdentityRole<long>, IEntity<long>
    {
        public IList<UserRole> Users { get; set; } = new List<UserRole>();

        public virtual bool IsTransient()
        {
            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext            
            return Convert.ToInt64(Id) <= 0;
        }
    }
}
