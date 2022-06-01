using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation
{
    public class EmployeeKYC : IEmployeeKYC
    {
       public virtual int Id { get; set; }
       public virtual string DoxType { get; set; }
       public virtual string DocumentNumber { get; set; }
       public virtual string NameonDox { get; set; }
       public virtual string Other { get; set; }
       public virtual DateTime? IssueDate { get; set; }
       public virtual DateTime? Exipiry { get; set; }
       public virtual string Place { get; set; }
    }
}
