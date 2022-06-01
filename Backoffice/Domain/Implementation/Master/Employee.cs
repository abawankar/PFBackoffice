using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation
{
    public class Employee : IEmployee
    {
        public virtual int Id { get; set; }
        public virtual string UAN { get; set; }
        public virtual string MemberId { get; set; }
        public virtual string EmpCode { get; set; }
        public virtual string Name { get; set; }
        public virtual string Gender { get; set; }
        public virtual string ForHName { get; set; }
        public virtual string ForHFlag { get; set; }
        public virtual DateTime? DOB { get; set; }
        public virtual DateTime? DOJ { get; set; }
        public virtual DateTime? DOE { get; set; }
        public virtual string Nationality { get; set; }
        public virtual string MaritalStatus { get; set; }
        public virtual string Mobile { get; set; }
        public virtual string EmailId { get; set; }
        public virtual string ReasonForLeaving { get; set; }
        public virtual string CellingEPF { get; set; }
        public virtual string CellingEPS { get; set; }
        public virtual ICompany Company { get; set; }
        public virtual IList<IEmployeeKYC> KYC { get; set; }
        public virtual int Status { get; set; }
        public virtual double VPF { get; set; }
        public virtual string PFExempted { get; set; }
        public virtual string ESIExempted { get; set; }
        public virtual string ESIIP { get; set; }
        public virtual int IPStatus { get; set; }
        public virtual string Branch { get; set; }
        public virtual int PMRPY { get; set; }

        public Employee()
        {
            KYC = new List<IEmployeeKYC>();
        }
    }
}
