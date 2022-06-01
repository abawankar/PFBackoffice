using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Master
{
    public class ESIEmp : IESIEmp
    {
        public virtual int Id { get; set; }
        public virtual string CompName { get; set; }
        public virtual string EmpCode { get; set; }
        public virtual string Name { get; set; }
        public virtual string FName { get; set; }
        public virtual DateTime DOB { get; set; }
        public virtual DateTime DOJ { get; set; }
        public virtual string Gender { get; set; }
        public virtual string PAddress { get; set; }
        public virtual string PrAddress { get; set; }
        public virtual string NName { get; set; }
        public virtual string NomRelation { get; set; }
        public virtual string NomAddress { get; set; }
        public virtual string OldIns { get; set; }
        public virtual int Status { get; set; }
    }
}
