using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample.Domain
{
    public class Permission : AggregateRoot
    {
        /// <summary>
        /// 动作Id
        /// </summary>
        public int ActionId { get; set; }

        /// <summary>
        /// 组织Id
        /// </summary>
        public int OrgId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public int ResourceId { get; set; }
    }
}
