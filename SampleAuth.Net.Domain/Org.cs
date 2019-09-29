using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample.Domain
{
    /// <summary>
    /// 组织架构实体
    /// </summary>
    public class Org : AggregateRoot<int>, ICascadable<Org, int>
    {
        /// <summary>
        /// 组织名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上级组织
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string IconName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 组织类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int SortNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public virtual Org Parent { get; set; }

        public virtual ICollection<Org> Children { get; set; }
    }
}
