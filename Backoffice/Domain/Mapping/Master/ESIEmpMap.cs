using Domain.Implementation.Master;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Master
{
    public class ESIEmpMap : ClassMap<ESIEmp>
    {
        public ESIEmpMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.CompName);
            Map(x => x.EmpCode);
            Map(x => x.Name);
            Map(x => x.FName);
            Map(x => x.DOB);
            Map(x => x.DOJ);
            Map(x => x.Gender);
            Map(x => x.PAddress);
            Map(x => x.PrAddress);
            Map(x => x.NName);
            Map(x => x.NomRelation);
            Map(x => x.NomAddress);
            Map(x => x.OldIns);
            Map(x => x.Status);
        }
    }
}
