using Domain.Implementation;
using Domain.Implementation.Transaction;
using FluentNHibernate.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Transaction
{
    public class MonthlyReturnMasterMap : ClassMap<MonthlyReturnMaster>
    {
        public MonthlyReturnMasterMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.ContType);
            Map(x => x.MonthYear);
            Map(x => x.TRRN);
            Map(x => x.CRN);
            Map(x => x.PaymentDate);
            References<Company>(x => x.Company).Column("CompId").LazyLoad();
            HasMany<MonthlyReturn>(x => x.MonthlyReturn).KeyColumn("ReturnMasterId").Cascade.All();

            Map(x => x.AdminAct2).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.Act10).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.EDLIAct21).CustomSqlType("numeric(12,4)").Not.Nullable();
        }
    }
}
