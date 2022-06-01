using Domain.Interface;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Transaction
{
    public class CMCTransaction : ICMCTransaction
    {
        public virtual int Id { get; set; }
        public virtual ICMCMember Member { get; set; }
        public virtual double Honorarium { get; set; }
        public virtual double Contingency { get; set; }
        public virtual double TAOPE { get; set; }
        public virtual double Review { get; set; }
        public virtual double Meeting { get; set; }
        public virtual double Claim { get; set; }
        public virtual string Remarks { get; set; }
    }
}
