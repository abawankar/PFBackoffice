using Domain.Implementation;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping
{
    public class LoginInfoMap : ClassMap<LoginInfo>
    {
        public LoginInfoMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.UserName);
            Map(x => x.Date);
        }
    }
}
