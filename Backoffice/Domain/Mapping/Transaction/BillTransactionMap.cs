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
    public class BillTransactionMap : ClassMap<BillTransaction>
    {
        public BillTransactionMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.MonthYear);
            Map(x => x.Year);
            Map(x => x.Amount).CustomSqlType("numeric(12,2)").Not.Nullable();
            References<ServiceName>(x => x.Service).Column("ServiceId").LazyLoad();

            Map(x => x.CGSTRate).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.CGST).CustomSqlType("numeric(12,2)").Not.Nullable();

            Map(x => x.SGSTRate).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.SGST).CustomSqlType("numeric(12,2)").Not.Nullable();

            Map(x => x.IGSTRate).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.IGST).CustomSqlType("numeric(12,2)").Not.Nullable();
        }
    }
}
