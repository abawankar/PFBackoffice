using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IAppsUser
    {
        int Id { get; set; }
        string Code { get; set; }
        string Name { get; set; }
        string Address { get; set; }
        ICompany Company { get; set; }
        string PAN { get; set; }
        DateTime? DOB { get; set; }
        string Emailid { get; set; }
        string MobileNo { get; set; }
        string AppLogin { get; set; }
        string Role { get; set; }
        IList<IUserRights> EmpRight { get; set; }
        IList<ICompany> AssignCompany { get; set; }
        int Status { get; set; }
    }
}
