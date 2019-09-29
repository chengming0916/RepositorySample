using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample.Domain
{
    /// <summary>
    /// 用户-组织关联表
    /// </summary>
    public class UserOrg : EntityBase
    {
        public Guid UserId { get; set; }

        public Guid OrgId { get; set; }
    }
}
