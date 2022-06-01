using Domain.Interface;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Transaction
{
    public class ESIMonthlyContMaster : IESIMonthlyContMaster
    {
        public virtual int Id { get; set; }
        public virtual ICompany Company { get; set; }
        public virtual string ContType { get; set; }
        public virtual string MonthYear { get; set; }
        public virtual double ERCont { get; set; }
        public virtual IList<IESIMonthlyCont> MonthlyCont { get; set; }
        public virtual string ChallanNo { get; set; }
        public virtual DateTime? Date { get; set; }
    }
}
