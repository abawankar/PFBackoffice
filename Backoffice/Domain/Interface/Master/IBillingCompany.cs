using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IBillingCompany
    {
        int Id { get; set; }
        string Name { get; set; }
        string Address { get; set; }
        string StateCode { get; set; }
        string GST { get; set; }
        string PAN { get; set; }
    }
}
