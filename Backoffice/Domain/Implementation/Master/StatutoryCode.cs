using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation
{
    public class StatutoryCode : IStatutoryCode
    {
       public virtual int Id { get; set; }
       public virtual string Code { get; set; }
       public virtual double PFPercent { get; set; }
       public virtual double AdminAct2 { get; set; }
       public virtual double Act10 { get; set; }
       public virtual double EDLIAct21 { get; set; }
       public virtual double AdminAct22 { get; set; }
       public virtual double AdminMinChar { get; set; }
       public virtual double EPSLimit { get; set; }
       public virtual double PFLimit { get; set; }
       public virtual double ESIEmpRate { get; set; }
       public virtual double ESIERRate { get; set; }
       public virtual double ESILimit { get; set; }
    }
}
