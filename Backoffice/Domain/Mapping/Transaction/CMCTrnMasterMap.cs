using Domain.Implementation.Transaction;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Transaction
{
    public class CMCTrnMasterMap : ClassMap<CMCTrnMaster>
    {
        public CMCTrnMasterMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.MonthYear);
            HasMany<CMCTransaction>(x => x.Trn).KeyColumn("MasterId").Cascade.All();
        }
    }
}
