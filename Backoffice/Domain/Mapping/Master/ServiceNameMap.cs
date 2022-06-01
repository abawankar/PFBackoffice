using Domain.Implementation;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping
{
    public class ServiceNameMap : ClassMap<ServiceName>
    {
        public ServiceNameMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.SAC);
            Map(x => x.Rate).CustomSqlType("numeric(12,2)").Not.Nullable();
        }
    }
}
