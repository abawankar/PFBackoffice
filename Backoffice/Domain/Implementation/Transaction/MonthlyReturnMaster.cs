using Domain.Interface;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Transaction
{
    public class MonthlyReturnMaster : IMonthlyReturnMaster
    {
        public virtual int Id { get; set; }
        public virtual ICompany Company { get; set; }
        public virtual string ContType { get; set; }
        public virtual string MonthYear { get; set; }
        public virtual string TRRN { get; set; }
        public virtual string CRN { get; set; }
        public virtual DateTime? PaymentDate { get; set; }
        public virtual IList<IMonthlyReturn> MonthlyReturn { get; set; }

        public virtual double AdminAct2 { get; set; }
        public virtual double Act10 { get; set; }
        public virtual double EDLIAct21 { get; set; }
    }
}
