using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface IMonthlyReturn
    {
        int Id { get; set; }
        string ContType { get; set; }
        string MonthYear { get; set; }
        string Name { get; set; }
        string EmpCode { get; set; }
        string UAN { get; set; }
        double GrossWages { get; set; }
        double EPFWages { get; set; }
        double EPSWages { get; set; }
        double EDLIWages { get; set; }
        double EECont { get; set; }
        double EPSCont { get; set; }
        double ERCont { get; set; }
        int NCPDays { get; set; }
        double ESIWages { get; set; }
        string Branch { get; set; }
    }

    public interface IMonthlyReturnDraft
    {
        int Id { get; set; }
        string ContType { get; set; }
        ICompany Company { get; set; }
        string MonthYear { get; set; }
        string Name { get; set; }
        string EmpCode { get; set; }
        string UAN { get; set; }
        double GrossWages { get; set; }
        double EPFWages { get; set; }
        double EPSWages { get; set; }
        double EDLIWages { get; set; }
        double EECont { get; set; }
        double EPSCont { get; set; }
        double ERCont { get; set; }
        int NCPDays { get; set; }
        double ESIWages { get; set; }
        string Branch { get; set; }
        string IsPF { get; set; }
    }
}
