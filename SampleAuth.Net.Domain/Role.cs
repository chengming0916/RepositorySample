using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample.Domain
{
    public class Role : AggregateRoot
    {
        public Role()
        {
            Name = string.Empty;
            Status = 0;
            Type = 0;
        }

        public string Name { get; set; }

        public int Status { get; set; }

        public int Type { get; set; }
    }
}
