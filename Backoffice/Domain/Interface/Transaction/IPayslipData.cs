using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface IPayslipMaster
    {
        int Id { get; set; }
        DateTime SalMonth { get; set; }
        int NoofEmp { get; set; }
        double? Basic { get; set; }
        double? Hra { get; set; }
        double? Conv { get; set; }
        double? Other { get; set; }
        double? Extra9 { get; set; }
        double? Extra12 { get; set; }
        double? Other6 { get; set; }
        double? GrossPay { get; set; }
        double? PfWorker { get; set; }
        double? EsiWorker { get; set; }
        double? TDS { get; set; }
        double? Advance { get; set; }
        double? TotalDedn { get; set; }
        double? NetPay { get; set; }
        IList<IPayslipData> PayslipData { get; set; }

    }
    public interface IPayslipData
    {
        int Id { get; set; }
        DateTime SalMonth { get; set; }
        string EmpCode { get; set; }
        string CardNo { get; set; }
        string GroupCode { get; set; }
        string PFNo { get; set; }
        string UAN { get; set; }
        string ESI { get; set; }
        string Name { get; set; }
        string FatherName { get; set; }
        int WorkDay { get; set; }
        int Holiday { get; set; }
        int WeekOff { get; set; }
        double? EBasic { get; set; }
        double? EHra { get; set; }
        double? EConv { get; set; }
        double? EOther { get; set; }
        double? ESpal { get; set; }
        double? EExtra { get; set; }
        double? TotalRate { get; set; }
        double? Basic { get; set; }
        double? Hra { get; set; }
        double? Conv { get; set; }
        double? Other { get; set; }
        double? Extra9 { get; set; }
        double? Extra12 { get; set; }
        double? Other6 { get; set; }
        double? GrossPay { get; set; }
        double? PfWorker { get; set; }
        double? EsiWorker { get; set; }
        double? TDS { get; set; }
        double? Advance { get; set; }
        double? TotalDedn { get; set; }
        double? NetPay { get; set; }
        string Designation { get; set; }
        DateTime? AppointDt { get; set; }
        string PAN { get; set; }
        int Dept { get; set; }
        string Emailid { get; set; }
        string BankName { get; set; }
        string BankAccount { get; set; }
        DateTime? DOB { get; set; }
        string Gender { get; set; }
        MailSent MailSent { get; set; }


    }

    public enum MailSent
    {
        No,
        Yes
    }
}
