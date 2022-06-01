using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IDigitalSignature
    {
        int Id { get; set; }
        string Name { get; set; }
        DateTime Validity { get; set; }
    }
}
