using DAL.Common;
using Domain.Interface;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Master
{
    public class EmployeeDAL : Common.CommonDAL<IEmployee>
    {
        public static bool DeleteBulkEmployee(int compid)
        {
            bool flag = false;
            ISession session1 = NHibernateHelper.OpenSession();
            try
            {
                var q1 = "delete Employee where Compid =" + compid ;
                var result1 = session1.CreateSQLQuery(q1).ExecuteUpdate();
                flag = true;
            }
            catch (Exception)
            {
                flag = false;
            }

            return flag;
        }
        public virtual IEmployee GetByUAN(string UAN)
        {
            IEmployee obj = NHibernateHelper
            .OpenSession()
            .CreateCriteria(typeof(IEmployee))
            .Add(NHibernate.Criterion.Restrictions.Eq("UAN",UAN))
            .SetFirstResult(0)
            .UniqueResult<IEmployee>();
            return obj;
        }

        public virtual IEmployee GetByCompAndCode(int compid,string empCode)
        {
            IEmployee obj = NHibernateHelper
            .OpenSession()
            .CreateCriteria(typeof(IEmployee))
            .Add(NHibernate.Criterion.Restrictions.Eq("EmpCode", empCode))
            .Add(NHibernate.Criterion.Restrictions.Eq("Company.Id", compid))
            .SetFirstResult(0)
            .UniqueResult<IEmployee>();
            return obj;
        }

        public virtual IList<IEmployee> GetByESICode(string empCode)
        {
            IList<IEmployee> obj = NHibernateHelper
            .OpenSession()
            .CreateCriteria(typeof(IEmployee))
            .Add(NHibernate.Criterion.Restrictions.Eq("EmpCode", empCode))
            .Add(NHibernate.Criterion.Restrictions.Eq("ESIExempted", "N"))
            .List<IEmployee>();
            return obj;
        }

        public static string GetBranch(int compid, string empCode)
        {
            IEmployee obj = NHibernateHelper
            .OpenSession()
            .CreateCriteria(typeof(IEmployee))
            .Add(NHibernate.Criterion.Restrictions.Eq("EmpCode", empCode))
            .Add(NHibernate.Criterion.Restrictions.Eq("Company.Id", compid))
            .SetFirstResult(0)
            .UniqueResult<IEmployee>();
            return obj.Branch;
        }

        public virtual IList<IEmployee> GetByCompId(int id)
        {
            IList<IEmployee> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(IEmployee))
               .Add(NHibernate.Criterion.Restrictions.Eq("Company.Id", id))
               .List<IEmployee>();
            return obj;
        }

        public virtual IList<IEmployee> GetESIByCompId(int id)
        {
            IList<IEmployee> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(IEmployee))
               .Add(NHibernate.Criterion.Restrictions.Eq("Company.Id", id))
               .Add(NHibernate.Criterion.Restrictions.Eq("ESIExempted", "N"))
               .List<IEmployee>();
            return obj;
        }

        public virtual IList<IEmployee> GetByCompId(int id,string esi)
        {
            IList<IEmployee> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(IEmployee))
               .Add(NHibernate.Criterion.Restrictions.Eq("Company.Id", id))
               .Add(NHibernate.Criterion.Restrictions.Eq("ESIExempted", esi))
               .List<IEmployee>();
            return obj;
        }
    }
}
