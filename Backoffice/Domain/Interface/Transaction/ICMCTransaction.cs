using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface ICMCTransaction
    {
        int Id { get; set; }
        ICMCMember Member { get; set; }
        double Honorarium{ get; set; }
        double Contingency { get; set; }
        double TAOPE { get; set; }
        double Review { get; set; }
        double Meeting { get; set; }
        double Claim { get; set; }
        string Remarks { get; set; }
    }
}
