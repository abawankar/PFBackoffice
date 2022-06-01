using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation
{
    public class AppsUser : IAppsUser
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual ICompany Company { get; set; }
        public virtual string PAN { get; set; }
        public virtual DateTime? DOB { get; set; }
        public virtual string Emailid { get; set; }
        public virtual string MobileNo { get; set; }
        public virtual string AppLogin { get; set; }
        public virtual string Role { get; set; }
        public virtual IList<IUserRights> EmpRight { get; set; }
        public virtual int Status { get; set; }
        public virtual IList<ICompany> AssignCompany { get; set; }
    }
}
