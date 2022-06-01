using Domain.Implementation.Transaction;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Transaction
{
    public  class ESIMonthlyContMap : ClassMap<ESIMonthlyCont>
    {
        public ESIMonthlyContMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.ContType);
            Map(x => x.MonthYear);
            Map(x => x.EmpCode);
            Map(x => x.Name);
            Map(x => x.IP);
            Map(x => x.GrossWages).CustomSqlType("numeric(12,2)").Not.Nullable();
            
            Map(x => x.IPCont).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.Days);
            Map(x => x.DOL);
            Map(x => x.Branch);
        }
    }
}
