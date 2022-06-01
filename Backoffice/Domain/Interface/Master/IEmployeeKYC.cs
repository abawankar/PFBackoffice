using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IEmployeeKYC
    {
        int Id { get; set; }
        string DoxType { get; set; }
        string DocumentNumber { get; set; }
        string NameonDox { get; set; }
        string Other { get; set; }
        DateTime? IssueDate { get; set; }
        DateTime? Exipiry { get; set; }
        string Place { get; set; }
    }
}
