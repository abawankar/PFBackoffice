using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation
{
    public class BillingCompany : IBillingCompany
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual string StateCode { get; set; }
        public virtual string GST { get; set; }
        public virtual string PAN { get; set; }
    }
}
