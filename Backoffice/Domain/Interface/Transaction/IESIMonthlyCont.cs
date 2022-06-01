using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface IESIMonthlyCont
    {
        int Id { get; set; }
        string ContType { get; set; }
        string MonthYear { get; set; }
        string EmpCode { get; set; }
        string Name { get; set; }
        string IP { get; set; }
        double GrossWages { get; set; }
        double IPCont { get; set; }
        int Days { get; set; }
        DateTime? DOL { get; set; }
        string Branch { get; set; }
    }
}
