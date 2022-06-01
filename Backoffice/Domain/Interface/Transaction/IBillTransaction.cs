using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface IBillTransaction
    {
        int Id { get; set; }
        IServiceName Service { get; set; }
        string MonthYear { get; set; }
        string Year { get; set; }
        double Amount { get; set; }
        double CGSTRate { get; set; }
        double CGST { get; set; }
        double SGSTRate { get; set; }
        double SGST { get; set; }
        double IGSTRate { get; set; }
        double IGST { get; set; }

    }
}
