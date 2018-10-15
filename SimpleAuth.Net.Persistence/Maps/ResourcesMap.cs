using RepositorySample.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample.Persistence.Maps
{
    public class ResourcesMap : EntityTypeConfiguration<Resource>
    {
        public ResourcesMap()
        {
            HasKey(x => x.Id).ToTable("t_resource");
        }
    }
}
