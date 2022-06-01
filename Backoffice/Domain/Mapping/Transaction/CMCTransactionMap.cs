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
    public class CMCTransactionMap : ClassMap<CMCTransaction>
    {
        public CMCTransactionMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Honorarium).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.Contingency).CustomSqlType("numeric(12,2)").Not.Nullable(); 
            Map(x=>x.TAOPE).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.Review).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.Meeting).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.Claim).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.Remarks);
            References<CMCMember>(x => x.Member).Column("MemberId").LazyLoad();
        }
    }
}
