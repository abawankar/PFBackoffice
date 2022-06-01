using Domain.Interface;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Transaction
{
    public class BillTransaction : IBillTransaction
    {
        public virtual int Id { get; set; }
        public virtual IServiceName Service { get; set; }
        public virtual string MonthYear { get; set; }
        public virtual string Year { get; set; }
        public virtual double Amount { get; set; }
        public virtual double CGSTRate { get; set; }
        public virtual double CGST { get; set; }
        public virtual double SGSTRate { get; set; }
        public virtual double SGST { get; set; }
        public virtual double IGSTRate { get; set; }
        public virtual double IGST { get; set; }
    }
}
