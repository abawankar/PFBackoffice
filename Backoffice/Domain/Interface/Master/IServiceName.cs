using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IServiceName
    {
        int Id { get; set; }
        string Name { get; set; }
        string SAC { get; set; }
        double Rate { get; set; }
    }
}
