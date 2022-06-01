using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IEmployee
    {
        int Id { get; set; }
        string UAN { get; set; }
        string MemberId { get; set; }
        string EmpCode { get; set; }
        string Name { get; set; }
        string Gender { get; set; }
        string ForHName { get; set; }
        string ForHFlag { get; set; }
        DateTime? DOB { get; set; }
        DateTime? DOJ { get; set; }
        DateTime? DOE { get; set; }
        string Nationality { get; set; }
        string MaritalStatus { get; set; }
        string Mobile { get; set; }
        string EmailId { get; set; }
        string ReasonForLeaving { get; set; }
        string CellingEPF { get; set; }
        string CellingEPS { get; set; }
        ICompany Company { get; set; }
        IList<IEmployeeKYC> KYC { get; set; }
        int Status { get; set; }
        double VPF { get; set; }
        string PFExempted { get; set; }
        string ESIExempted { get; set; }
        string ESIIP { get; set; }
        int IPStatus { get; set; }
        string Branch { get; set; }
        int PMRPY { get; set; }
    }
}
