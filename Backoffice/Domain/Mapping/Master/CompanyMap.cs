using Domain.Implementation;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping
{
    public sealed class CompanyMap : ClassMap<Company>
    {
        public CompanyMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Code);
            Map(x => x.Name);
            Map(x => x.Address);
            Map(x => x.StateCode);
            Map(x => x.PAN);
            Map(x => x.GST);
            Map(x => x.ServiceTax);
            Map(x => x.Emailid);
            Map(x => x.PhoneNo);
            Map(x => x.CIN);
            Map(x => x.RegDate);
            Map(x => x.RegNumber);
            Map(x => x.WebSite);
            Map(x => x.EstablishmentCode);
            Map(x => x.ESIRegistrationNumber);
            HasMany<DigitalSignature>(x => x.DigitalSign).KeyColumn("CompId").Cascade.All();
            References<StatutoryCode>(x => x.StatutoryCode).Column("StatCodeId").ForeignKey("fk_complany_statcodeid").LazyLoad();

        }
    }
}
