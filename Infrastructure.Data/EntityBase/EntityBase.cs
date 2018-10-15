using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{

    public interface IEntity { }

    public interface IEntity<TKey> : IEntity
        where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }

    public abstract class EntityBase<TKey> : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
    }

    public abstract class EntityBase : EntityBase<int> { }
}
