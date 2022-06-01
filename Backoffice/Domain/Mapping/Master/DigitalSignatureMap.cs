using Domain.Implementation;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping
{
    public class DigitalSignatureMap : ClassMap<DigitalSignature>
    {
        public DigitalSignatureMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Validity);
        }
    }
}
