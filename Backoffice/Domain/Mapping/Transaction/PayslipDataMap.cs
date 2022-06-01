using Domain.Implementation.Transaction;
using Domain.Interface.Transaction;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Transaction
{
    public class PayslipDataMap : ClassMap<PayslipData>
    {
        public PayslipDataMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.SalMonth);
            Map(x=>x.EmpCode);
            Map(x=>x.CardNo);
            Map(x=>x.GroupCode);
            Map(x=>x.PFNo);
            Map(x=>x.UAN);
            Map(x=>x.ESI);
            Map(x=>x.Name);
            Map(x=>x.FatherName);
            Map(x=>x.WorkDay);
            Map(x=>x.Holiday);
            Map(x=>x.WeekOff);
            Map(x=>x.Basic).CustomSqlType("numeric(12,2)").Not.Nullable(); 
            Map(x=>x.Hra).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.Conv).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.Other).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.Extra9).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.Extra12).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.Other6).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.GrossPay).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.PfWorker).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.EsiWorker).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.TDS).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.Advance).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.TotalDedn).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.NetPay).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.Designation);
            Map(x => x.AppointDt);
            Map(x => x.PAN);
            Map(x => x.Dept);
            Map(x => x.Emailid);
            Map(x => x.BankName);
            Map(x => x.BankAccount);
            Map(x => x.DOB);
            Map(x => x.MailSent).CustomType<MailSent>();

        }
    }

    public class PayslipMasterMap : ClassMap<PayslipMaster>
    {
        public PayslipMasterMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.SalMonth);
            Map(x => x.NoofEmp);
            Map(x=>x.Basic).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.Hra).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.Conv).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.Other).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.Extra9).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.Extra12).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.Other6).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.GrossPay).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.PfWorker).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.EsiWorker).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.TDS).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.Advance).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.TotalDedn).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x=>x.NetPay).CustomSqlType("numeric(12,2)").Not.Nullable();
            HasMany<PayslipData>(x => x.PayslipData).KeyColumn("PayslipMasterid").Cascade.All();
        }

    }
}
