using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Transaction
{
    public class CMCTrnMaster : ICMCTrnMaster
    {
        public virtual int Id { get; set; }
        public virtual string MonthYear { get; set; }
        public virtual IList<ICMCTransaction> Trn { get; set; }
    }
}
