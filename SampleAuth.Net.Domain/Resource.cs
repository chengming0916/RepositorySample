using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample.Domain
{
    /// <summary>
    /// 资源实体
    /// </summary>
    public class Resource : AggregateRoot<int>, ICascadable<Resource, int>
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public int ParentId { get; set; }

        public Resource Parent { get; set; }
        public ICollection<Resource> Children { get; set; }

    }
}
