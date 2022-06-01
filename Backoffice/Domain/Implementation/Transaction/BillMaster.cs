using Domain.Interface;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Transaction
{
    public class BillMaster : IBillMaster
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string BillNo { get; set; }
        public virtual ICompany Company { get; set; }
        public virtual IBillingCompany BillingCompany { get; set; }
        public virtual IList<IBillTransaction> Tran { get; set; }
        public virtual int Status { get; set; }
    }
}
