using DAL.Common;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Transaction
{
    public class ESIMonthlyContMasterDAL : Common.CommonDAL<IESIMonthlyContMaster>
    {
        public IList<IESIMonthlyContMaster> GetByCompId(int id)
        {
            IList<IESIMonthlyContMaster> obj = NHibernateHelper.OpenSession()
              .CreateCriteria(typeof(IESIMonthlyContMaster))
              .Add(NHibernate.Criterion.Restrictions.Eq("Company.Id", id))
             .List<IESIMonthlyContMaster>();
            return obj;
        }
    }
}
