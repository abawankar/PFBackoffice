using DAL.Common;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Master
{
    public class CMCMemberDAL : Common.CommonDAL<ICMCMember>
    {
        public static ICMCMember GetMember(string code)
        {
            ICMCMember obj = NHibernateHelper
            .OpenSession()
            .CreateCriteria(typeof(ICMCMember))
            .Add(NHibernate.Criterion.Restrictions.Eq("Code", code))
            .SetFirstResult(0)
            .UniqueResult<ICMCMember>();
            return obj;
        }
    }
}
