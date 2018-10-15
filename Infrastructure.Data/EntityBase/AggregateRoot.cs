using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public interface IAggregateRoot { }

    public interface IAggregateRoot<TKey> : IAggregateRoot, IEntity
        where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }

    public abstract class AggregateRoot<TKey> : IAggregateRoot<TKey>, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
    }

    public abstract class AggregateRoot : AggregateRoot<Guid>
    {

    }
}
