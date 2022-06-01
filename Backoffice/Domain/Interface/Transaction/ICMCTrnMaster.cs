using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface ICMCTrnMaster
    {
        int Id { get; set; }
        string MonthYear { get; set; }
        IList<ICMCTransaction> Trn { get; set; }
    }
}
