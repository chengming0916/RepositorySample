using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public abstract class EntityBase<TKey> where TKey : IEquatable<TKey>
    {
        public virtual TKey Id { get; set; }
    }

    public abstract class EntityBase : EntityBase<int>
    {

    }
}
