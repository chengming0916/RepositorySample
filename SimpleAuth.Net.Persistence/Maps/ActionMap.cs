using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample.Persistence.Maps
{
    class ActionMap : EntityTypeConfiguration<Domain.Action>
    {
        public ActionMap()
        {
            HasKey(x => x.Id).ToTable("t_action");
        }
    }
}
