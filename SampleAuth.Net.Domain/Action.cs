using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample.Domain
{
    /// <summary>
    /// 动作实体
    /// </summary>
    public class Action : AggregateRoot
    {
        /// <summary>
        /// 动作名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
