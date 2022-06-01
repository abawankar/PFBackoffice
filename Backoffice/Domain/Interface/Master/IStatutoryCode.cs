using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IStatutoryCode
    {
        int Id { get; set; }
        string Code { get; set; }
        double PFPercent { get; set; }
        double AdminAct2 { get; set; }
        double Act10 { get; set; }
        double EDLIAct21 { get; set; }
        double AdminAct22 { get; set; }
        double AdminMinChar { get; set; }
        double EPSLimit { get; set; }
        double PFLimit { get; set; }
        double ESIEmpRate { get; set; }
        double ESIERRate { get; set; }
        double ESILimit { get; set; }
    }
}
