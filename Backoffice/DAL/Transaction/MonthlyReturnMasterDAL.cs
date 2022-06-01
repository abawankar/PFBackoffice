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
    public class MonthlyReturnMasterDAL : Common.CommonDAL<IMonthlyReturnMaster>
    {
        public bool DeleteDraftReturn(string month, int compid, string ecr)
        {
            bool flag = false;
            ISession session1 = NHibernateHelper.OpenSession();
            try
            {
                var q1 = "delete MonthlyReturnDraft where Compid =" + compid + " and MonthYear='" + month + "' and ContType='" + ecr + "'";
                var result1 = session1.CreateSQLQuery(q1).ExecuteUpdate();
                flag = true;
            }
            catch (Exception)
            {
                flag = false;
            }

            return flag;
        }

        public bool UpdateMissingUAN(string month, int compid, string ecr)
        {
            bool flag = false;
            ISession session1 = NHibernateHelper.OpenSession();
            try
            {
                var q2 = "update MonthlyReturnDraft set ContType='S' where Compid =" + compid + " and MonthYear='" + month + "' and ContType='" + ecr + "' and (ISPF='N' and len(uan)<>12)";
                var result1 = session1.CreateSQLQuery(q2).ExecuteUpdate();
                flag = true;
            }
            catch (Exception)
            {
                flag = false;
            }

            return flag;
        }

        public IMonthlyReturnMaster GetByCompIdMonthEcr(string month, int id, string ecr)
        {
             IMonthlyReturnMaster obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(IMonthlyReturnMaster))
               .Add(NHibernate.Criterion.Restrictions.Eq("Company.Id", id))
               .Add(NHibernate.Criterion.Restrictions.Eq("MonthYear", month))
               .Add(NHibernate.Criterion.Restrictions.Eq("ContType", ecr))
               .SetFirstResult(0)
              .UniqueResult<IMonthlyReturnMaster>();
            return obj;
        }

        public IList<IMonthlyReturnMaster> GetByCompIdMonth(string month, int id)
        {
            IList<IMonthlyReturnMaster> obj = NHibernateHelper.OpenSession()
              .CreateCriteria(typeof(IMonthlyReturnMaster))
              .Add(NHibernate.Criterion.Restrictions.Eq("Company.Id", id))
              .Add(NHibernate.Criterion.Restrictions.Eq("MonthYear", month))
              .List<IMonthlyReturnMaster>();
            return obj;
        }

        public IList<IMonthlyReturnMaster> GetByCompId(int id)
        {
            IList<IMonthlyReturnMaster> obj = NHibernateHelper.OpenSession()
            .CreateCriteria(typeof(IMonthlyReturnMaster))
            .Add(NHibernate.Criterion.Restrictions.Eq("Company.Id", id))
           .List<IMonthlyReturnMaster>();
            return obj;
        }

        public IList<IMonthlyReturnMaster> NotPaid()
        {
            IList<IMonthlyReturnMaster> obj = NHibernateHelper.OpenSession()
            .CreateCriteria(typeof(IMonthlyReturnMaster))
            .Add(NHibernate.Criterion.Restrictions.IsNull("PaymentDate"))
           .List<IMonthlyReturnMaster>();
            return obj;
        }
    }
}
