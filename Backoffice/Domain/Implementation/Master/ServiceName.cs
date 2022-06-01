using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation
{
    public class ServiceName : IServiceName
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string SAC { get; set; }
        public virtual double Rate { get; set; }
    }
}
