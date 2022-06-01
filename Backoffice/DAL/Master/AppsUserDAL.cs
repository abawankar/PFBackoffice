
using DAL.Common;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Master
{
    public class AppsUserDAL : Common.CommonDAL<IAppsUser>
    {
        public static IAppsUser GetByAppLogin(string Emailid)
        {
            IAppsUser obj = NHibernateHelper
            .OpenSession()
            .CreateCriteria(typeof(IAppsUser))
            .Add(NHibernate.Criterion.Restrictions.Eq("Emailid", Emailid))
            .SetFirstResult(0)
            .UniqueResult<IAppsUser>();

            return obj;
        }

        public static string GetName(string Emailid)
        {
            IAppsUser obj = NHibernateHelper
            .OpenSession()
            .CreateCriteria(typeof(IAppsUser))
            .Add(NHibernate.Criterion.Restrictions.Eq("Emailid", Emailid))
            .SetFirstResult(0)
            .UniqueResult<IAppsUser>();

            return obj.Name;
        }

        public void AddRights(int empId, string rightsList)
        {
            IAppsUser bl = GetById(empId);
            UserRightsDAL dal = new UserRightsDAL();
            string[] rightsid = rightsList.Split(',');
            for (int i = 1; i < rightsid.Length; i++)
            {
                int id = Convert.ToInt32(rightsid[i]);
                IUserRights empRights = dal.GetById(id);
                bl.EmpRight.Add(empRights);
            }
            InsertOrUpdate(bl);
        }

        public void AssignCompany(int empId, string compList)
        {
            IAppsUser bl = GetById(empId);
            CompanyDAL dal = new CompanyDAL();
            string[] compid = compList.Split(',');
            for (int i = 1; i < compid.Length; i++)
            {
                int id = Convert.ToInt32(compid[i]);
                ICompany empRights = dal.GetById(id);
                bl.AssignCompany.Add(empRights);
            }
            InsertOrUpdate(bl);
        }

    }
}
