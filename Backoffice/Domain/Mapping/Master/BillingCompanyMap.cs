using Domain.Implementation;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Master
{
    public class BillingCompanyMap: ClassMap<BillingCompany>
    {
        public BillingCompanyMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.Address);
            Map(x => x.StateCode);
            Map(x => x.GST);
            Map(x => x.PAN);
        }
    }
}
