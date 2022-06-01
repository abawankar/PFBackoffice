using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation
{
    public class CMCMember : ICMCMember
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string FatherName { get; set; }
        public virtual string Gender { get; set; }
        public virtual string DutyStation { get; set; }
        public virtual string District { get; set; }
        public virtual string SubRegion { get; set; }
        public virtual DateTime? DOB { get; set; }
        public virtual DateTime? DOC { get; set; }
        public virtual string BankAc { get; set; }
        public virtual string IFSC { get; set; }
        public virtual string BankName { get; set; }
        public virtual string PAN { get; set; }
        public virtual string Aadhaar { get; set; }
        public virtual string Mobile { get; set; }
        public virtual DateTime? DOE { get; set; }
    }
}
