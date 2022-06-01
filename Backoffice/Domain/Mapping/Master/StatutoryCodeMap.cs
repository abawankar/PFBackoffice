using Domain.Implementation;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping
{
    public class StatutoryCodeMap : ClassMap<StatutoryCode>
    {
        public StatutoryCodeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Code);
            Map(x => x.PFPercent).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.AdminAct2).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.Act10).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.EDLIAct21).CustomSqlType("numeric(12,4)").Not.Nullable();
            Map(x => x.AdminAct22).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.AdminMinChar).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.EPSLimit).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.PFLimit).CustomSqlType("numeric(12,2)").Not.Nullable();

            Map(x => x.ESIEmpRate).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.ESIERRate).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.ESILimit).CustomSqlType("numeric(12,2)").Not.Nullable();
        }
    }
}
