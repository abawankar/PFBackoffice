using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Transaction
{
    public class ESIMonthlyCont : IESIMonthlyCont
    {
        public virtual int Id { get; set; }
        public virtual string ContType { get; set; }
        public virtual string MonthYear { get; set; }
        public virtual string EmpCode { get; set; }
        public virtual string Name { get; set; }
        public virtual string IP { get; set; }
        public virtual double GrossWages { get; set; }
        public virtual double IPCont { get; set; }
        public virtual int Days { get; set; }
        public virtual DateTime? DOL { get; set; }
        public virtual string Branch { get; set; }
    }
}
