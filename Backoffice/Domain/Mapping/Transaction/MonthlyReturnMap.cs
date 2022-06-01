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
    public class MonthlyReturnMap : ClassMap<MonthlyReturn>
    {
        public MonthlyReturnMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.ContType);
            Map(x => x.MonthYear);
            Map(x => x.Name);
            Map(x => x.EmpCode);
            Map(x => x.UAN);
            Map(x => x.GrossWages).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.EPFWages).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.EPSWages).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.EDLIWages).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.EECont).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.EPSCont).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.ERCont).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.NCPDays);
            Map(x => x.ESIWages).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.Branch);
        }
       
    }

    public class MonthlyReturnDraftMap : ClassMap<MonthlyReturnDraft>
    {
        public MonthlyReturnDraftMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.ContType);
            Map(x => x.MonthYear);
            Map(x => x.Name);
            Map(x => x.EmpCode);
            Map(x => x.UAN);
            Map(x => x.GrossWages).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.EPFWages).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.EPSWages).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.EDLIWages).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.EECont).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.EPSCont).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.ERCont).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.NCPDays);
            Map(x => x.ESIWages).CustomSqlType("numeric(12,2)").Not.Nullable();
            References<Company>(x => x.Company).Column("CompId").ForeignKey("fk_monthlytrandraft_compid").LazyLoad();
            Map(x => x.Branch);
            Map(x => x.IsPF);
        }

    }
}
