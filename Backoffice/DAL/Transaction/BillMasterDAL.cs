using DAL.Common;
using Domain.Interface.Transaction;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Transaction
{
    public class BillMasterDAL : Common.CommonDAL<IBillMaster>
    {
        public int GetMaxBillNumber(int compid)
        {
            var obj = NHibernateHelper.OpenSession()
             .CreateCriteria(typeof(IBillMaster))
             .Add(NHibernate.Criterion.Restrictions.Eq("BillingCompany.Id", compid))
             .SetProjection(Projections.Max("BillNo"))
             .UniqueResult();
            if (obj != null)
            {
                IBillMaster obj1 = NHibernateHelper
               .OpenSession()
               .CreateCriteria(typeof(IBillMaster))
               .Add(NHibernate.Criterion.Restrictions.Eq("BillNo", obj))
               .SetFirstResult(0)
               .UniqueResult<IBillMaster>();

                return Convert.ToInt32(obj1.BillNo);
            }
            else
                return 10000;
        }
    }
}
