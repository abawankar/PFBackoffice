using Domain.Interface;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Implementation.Transaction
{
    public class MonthlyReturn : IMonthlyReturn
    {
        public virtual int Id { get; set; }
        public virtual string ContType { get; set; }
        public virtual string MonthYear { get; set; }
        public virtual string Name { get; set; }
        public virtual string EmpCode { get; set; }
        public virtual string UAN { get; set; }
        public virtual double GrossWages { get; set; }
        public virtual double EPFWages { get; set; }
        public virtual double EPSWages { get; set; }
        public virtual double EDLIWages { get; set; }
        public virtual double EECont { get; set; }
        public virtual double EPSCont { get; set; }
        public virtual double ERCont { get; set; }
        public virtual int NCPDays { get; set; }
        public virtual double ESIWages { get; set; }
        public virtual string Branch { get; set; }

    }

    public class MonthlyReturnDraft : IMonthlyReturnDraft
    {
        public virtual int Id { get; set; }
        public virtual string ContType { get; set; }
        public virtual ICompany Company { get; set; }
        public virtual string MonthYear { get; set; }
        public virtual string Name { get; set; }
        public virtual string EmpCode { get; set; }
        public virtual string UAN { get; set; }
        public virtual double GrossWages { get; set; }
        public virtual double EPFWages { get; set; }
        public virtual double EPSWages { get; set; }
        public virtual double EDLIWages { get; set; }
        public virtual double EECont { get; set; }
        public virtual double EPSCont { get; set; }
        public virtual double ERCont { get; set; }
        public virtual int NCPDays { get; set; }
        public virtual double ESIWages { get; set; }
        public virtual string Branch { get; set; }
        public virtual string IsPF { get; set; }

    }
}
