using RepositorySample.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample.Persistence.Maps
{
    class OrganizationMap : EntityTypeConfiguration<Org>
    {
        public OrganizationMap()
        {
            HasKey(x => x.Id).ToTable("t_org");
        }
    }
}
