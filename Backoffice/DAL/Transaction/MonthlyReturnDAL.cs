using DAL.Common;
using Domain.Interface.Transaction;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Transaction
{
    public class MonthlyReturnDAL : Common.CommonDAL<IMonthlyReturn>
    {
        public IList<IMonthlyReturn> GetByCompIdMonthEcr(string month, int id, string ecr)
        {
            IList<IMonthlyReturn> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(IMonthlyReturn))
               .Add(NHibernate.Criterion.Restrictions.Eq("Company.Id", id))
               .Add(NHibernate.Criterion.Restrictions.Eq("MonthYear", month))
               .Add(NHibernate.Criterion.Restrictions.Eq("ContType", ecr))
               .List<IMonthlyReturn>();
            return obj;
        }

        public IList<IMonthlyReturn> GetByCompIdMonth(string month, int id)
        {
            IList<IMonthlyReturn> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(IMonthlyReturn))
               .Add(NHibernate.Criterion.Restrictions.Eq("Company.Id", id))
               .Add(NHibernate.Criterion.Restrictions.Eq("MonthYear", month))
               .List<IMonthlyReturn>();
            return obj;
        }

    }
    public class MonthlyReturnDraftDAL : Common.CommonDAL<IMonthlyReturnDraft>
    {
        public IList<IMonthlyReturnDraft> GetByCompIdMonthEcr(string month, int id, string ecr)
        {
            IList<IMonthlyReturnDraft> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(IMonthlyReturnDraft))
               .Add(NHibernate.Criterion.Restrictions.Eq("Company.Id", id))
               .Add(NHibernate.Criterion.Restrictions.Eq("MonthYear", month))
               .Add(NHibernate.Criterion.Restrictions.Eq("ContType", ecr))
               .List<IMonthlyReturnDraft>();
            return obj;
        }
    }
}
