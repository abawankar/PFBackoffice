using Domain.Implementation;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.UAN);
            Map(x => x.MemberId);
            Map(x => x.EmpCode);
            Map(x => x.Name);
            Map(x => x.Gender).CustomSqlType("varchar(1)");
            Map(x => x.ForHName);
            Map(x => x.ForHFlag).CustomSqlType("varchar(1)");
            Map(x => x.DOB);
            Map(x => x.DOJ);
            Map(x => x.DOE);
            Map(x => x.Nationality);
            Map(x => x.MaritalStatus).CustomSqlType("varchar(1)");
            Map(x => x.Mobile);
            Map(x => x.EmailId);
            Map(x => x.ReasonForLeaving);
            Map(x => x.Status);
            Map(x => x.VPF);
            Map(x => x.PFExempted).CustomSqlType("varchar(1)");
            Map(x => x.ESIExempted).CustomSqlType("varchar(1)"); 
            Map(x => x.CellingEPF).CustomSqlType("varchar(1)");
            Map(x => x.CellingEPS).CustomSqlType("varchar(1)");
            References<Company>(x => x.Company).Column("CompId").ForeignKey("fk_employee_compid").LazyLoad();
            HasMany<EmployeeKYC>(x => x.KYC).KeyColumn("EmpId").Cascade.All();

            Map(x => x.ESIIP);
            Map(x => x.IPStatus);
            Map(x => x.Branch);
            Map(x => x.PMRPY);
        }
    }
}
