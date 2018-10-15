using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    /// <summary>
    /// 可级联对象接口
    /// </summary>
    public interface ICascadable<TKey>
        where TKey : IEquatable<TKey>
    {
        TKey ParentId { get; set; }
    }

    public interface ICascadable<TEntity, TKey> : ICascadable<TKey>
        where TEntity : IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        TEntity Parent { get; set; }

        ICollection<TEntity> Children { get; set; }
    }
}
