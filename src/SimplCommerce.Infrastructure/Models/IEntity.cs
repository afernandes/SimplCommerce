using System;
using System.Collections.Generic;
using System.Text;

namespace SimplCommerce.Infrastructure.Models
{
    /// <summary>
    /// A shortcut of <see cref="IEntity{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    public interface IEntity : IEntity<long>
    {

    }
}
