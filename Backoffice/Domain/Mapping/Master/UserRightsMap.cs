using Domain.Implementation;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping
{
    public sealed class UserRightsMap : ClassMap<UserRights>
    {
        public UserRightsMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Code);
            Map(x => x.MnuName);
            Map(x => x.TableName);
            Map(x => x.Operation);
            Map(x => x.Description);
        }
    }
}
