using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Master
{
    public interface IESIEmp
    {
        int Id { get; set; }
        string CompName { get; set; }
        string EmpCode { get; set; }
        string Name { get; set; }
        string FName { get; set; }
        DateTime DOB { get; set; }
        DateTime DOJ { get; set; }
        string Gender { get; set; }
        string PAddress { get; set; }
        string PrAddress { get; set; }
        string NName { get; set; }
        string NomRelation { get; set; }
        string NomAddress { get; set; }
        string OldIns { get; set; }
        int Status { get; set; }

    }
}
