using Domain.Implementation;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Master
{
    public sealed class CMCMemberMap : ClassMap<CMCMember>
    {
        public CMCMemberMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Code);
            Map(x => x.Gender);
            Map(x => x.Name);
            Map(x => x.FatherName);
            Map(x => x.DutyStation);
            Map(x => x.District);
            Map(x => x.SubRegion);
            Map(x => x.DOB);
            Map(x => x.DOC);
            Map(x => x.BankAc);
            Map(x => x.IFSC);
            Map(x => x.BankName);
            Map(x => x.PAN);
            Map(x => x.Aadhaar);
            Map(x => x.Mobile);
            Map(x => x.DOE);
        }
    }
}
