using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation
{
    public class DigitalSignature : IDigitalSignature
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime Validity { get; set; }
    }
}
