using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface IESIMonthlyContMaster
    {
        int Id { get; set; }
        ICompany Company { get; set; }
        string ContType { get; set; }
        string MonthYear { get; set; }
        double ERCont { get; set; }
        IList<IESIMonthlyCont> MonthlyCont { get; set; }
        string ChallanNo { get; set; }
        DateTime? Date { get; set; }
    }
}
