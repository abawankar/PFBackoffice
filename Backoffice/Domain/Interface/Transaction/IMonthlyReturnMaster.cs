using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface IMonthlyReturnMaster
    {
        int Id { get; set; }
        ICompany Company { get; set; }
        string ContType { get; set; }
        string MonthYear { get; set; }
        string TRRN { get; set; }
        string CRN { get; set; }
        DateTime? PaymentDate { get; set; }
        IList<IMonthlyReturn>MonthlyReturn { get; set; }

        double AdminAct2 { get; set; }
        double Act10 { get; set; }
        double EDLIAct21 { get; set; }

    }
}
