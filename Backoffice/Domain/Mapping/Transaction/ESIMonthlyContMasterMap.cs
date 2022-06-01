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
    public class ESIMonthlyContMasterMap : ClassMap<ESIMonthlyContMaster>
    {
        public ESIMonthlyContMasterMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.ContType);
            Map(x => x.MonthYear);
            Map(x => x.ChallanNo);
            Map(x => x.Date);
            Map(x => x.ERCont).CustomSqlType("numeric(12,2)").Not.Nullable();
            References<Company>(x => x.Company).Column("CompId").LazyLoad();
            HasMany<ESIMonthlyCont>(x => x.MonthlyCont).KeyColumn("MasterId").Cascade.All();
        }
    }
}
