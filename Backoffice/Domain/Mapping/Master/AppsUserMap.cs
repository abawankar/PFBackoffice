using Domain.Implementation;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping
{
    public class AppsUserMap : ClassMap<AppsUser>
    {
        public AppsUserMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Code);
            Map(x => x.Name);
            Map(x => x.Address);
            Map(x => x.PAN);
            Map(x => x.DOB);
            Map(x => x.Emailid);
            Map(x => x.MobileNo);
            Map(x => x.Status);
            Map(x => x.AppLogin);
            Map(x => x.Role);
            HasManyToMany<UserRights>(x => x.EmpRight).ParentKeyColumn("EmpId").ChildKeyColumn("RightsId").LazyLoad();
            HasManyToMany<Company>(x => x.AssignCompany).ParentKeyColumn("EmpId").ChildKeyColumn("CompId").LazyLoad();
        }
    }
}
