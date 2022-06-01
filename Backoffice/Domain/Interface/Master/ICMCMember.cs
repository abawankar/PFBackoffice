using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ICMCMember
    {
        int Id { get; set; }
        string Code { get; set; }
        string Name { get; set; }
        string FatherName { get; set; }
        string Gender { get; set; }
        string DutyStation { get; set; }
        string District { get; set; }
        string SubRegion { get; set; }
        DateTime? DOB { get; set; }
        DateTime? DOC { get; set; }
        string BankAc { get; set; }
        string IFSC { get; set; }
        string BankName { get; set; }
        string PAN { get; set; }
        string Aadhaar { get; set; }
        string Mobile { get; set; }
        DateTime? DOE { get; set; }
    }
}
