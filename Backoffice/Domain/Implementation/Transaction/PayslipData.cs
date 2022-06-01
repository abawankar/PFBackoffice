using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Transaction
{
    public class PayslipMaster : IPayslipMaster
    {
       public virtual int Id { get; set; }
       public virtual DateTime SalMonth { get; set; }
       public virtual int NoofEmp { get; set; }
       public virtual double? Basic { get; set; }
       public virtual double? Hra { get; set; }
       public virtual double? Conv { get; set; }
       public virtual double? Other { get; set; }
       public virtual double? Extra9 { get; set; }
       public virtual double? Extra12 { get; set; }
       public virtual double? Other6 { get; set; }
       public virtual double? GrossPay { get; set; }
       public virtual double? PfWorker { get; set; }
       public virtual double? EsiWorker { get; set; }
       public virtual double? TDS { get; set; }
       public virtual double? TotalDedn { get; set; }
       public virtual double? NetPay { get; set; }
       public virtual double? Advance { get; set; }
       public virtual IList<IPayslipData> PayslipData { get; set; }

    }
    public class PayslipData : IPayslipData
    {
        public virtual int Id { get; set; }
        public virtual DateTime SalMonth { get; set; }
        public virtual string EmpCode { get; set; }
        public virtual string CardNo { get; set; }
        public virtual string GroupCode { get; set; }
        public virtual string PFNo { get; set; }
        public virtual string UAN { get; set; }
        public virtual string ESI { get; set; }
        public virtual string Name { get; set; }
        public virtual string FatherName { get; set; }
        public virtual int WorkDay { get; set; }
        public virtual int Holiday { get; set; }
        public virtual int WeekOff { get; set; }
        public virtual double? EBasic { get; set; }
        public virtual double? EHra { get; set; }
        public virtual double? EConv { get; set; }
        public virtual double? EOther { get; set; }
        public virtual double? ESpal { get; set; }
        public virtual double? EExtra { get; set; }
        public virtual double? TotalRate { get; set; }
        public virtual double? Basic { get; set; }
        public virtual double? Hra { get; set; }
        public virtual double? Conv { get; set; }
        public virtual double? Other { get; set; }
        public virtual double? Extra9 { get; set; }
        public virtual double? Extra12 { get; set; }
        public virtual double? Other6 { get; set; }
        public virtual double? GrossPay { get; set; }
        public virtual double? PfWorker { get; set; }
        public virtual double? EsiWorker { get; set; }
        public virtual double? TDS { get; set; }
        public virtual double? Advance { get; set; }
        public virtual double? TotalDedn { get; set; }
        public virtual double? NetPay { get; set; }
        public virtual string Designation { get; set; }
        public virtual DateTime? AppointDt { get; set; }
        public virtual string PAN { get; set; }
        public virtual int Dept { get; set; }
        public virtual string Emailid { get; set; }
        public virtual string BankName { get; set; }
        public virtual string BankAccount { get; set; }
        public virtual DateTime? DOB { get; set; }
        public virtual string Gender { get; set; }
        public virtual MailSent MailSent { get; set; }
    }
}
