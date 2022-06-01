using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface IBillMaster
    {
        int Id { get; set; }
        DateTime Date { get; set; }
        string BillNo { get; set; }
        ICompany Company { get; set; }
        IBillingCompany BillingCompany { get; set; }
        IList<IBillTransaction> Tran { get; set; }
        int Status { get; set; }
    }
}
