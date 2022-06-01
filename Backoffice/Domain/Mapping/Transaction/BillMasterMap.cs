using Domain.Implementation;
using Domain.Implementation.Transaction;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Transaction
{
    public class BillMasterMap : ClassMap<BillMaster>
    {
        public BillMasterMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Date);
            Map(x => x.BillNo);
            References<Company>(x => x.Company).Column("CompId").LazyLoad();
            References<BillingCompany>(x => x.BillingCompany).Column("BillingCompId").LazyLoad();
            HasMany<BillTransaction>(x => x.Tran).KeyColumn("BillId").Cascade.All();
            Map(x => x.Status);
        }
    }
}
