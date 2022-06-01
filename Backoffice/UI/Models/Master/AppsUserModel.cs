using DAL.Master;
using Domain.Implementation;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Master
{
    public class AppsUserModel : AppsUser
    {
        public int CompId { get; set; }

    }

    public class AppsUserRepository : RepositoryModel<AppsUserModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(AppsUserModel obj)
        {
            AppsUserDAL dal = new AppsUserDAL();
            IAppsUser bl = dal.GetById(obj.Id);
            bl.Code = obj.Code;
            bl.Name = obj.Name;
            bl.Address = obj.Address;
            bl.PAN = obj.PAN;
            bl.DOB = obj.DOB;
            bl.Emailid = obj.Emailid;
            bl.AppLogin = obj.Emailid;
            bl.MobileNo = obj.MobileNo;
            bl.Status = obj.Status;

            dal.InsertOrUpdate(bl);
        }

        public override IList<AppsUserModel> GetAll()
        {
            AppsUserDAL dal = new AppsUserDAL();
            AutoMapper.Mapper.CreateMap<AppsUser, AppsUserModel>();
            AutoMapper.Mapper.CreateMap<AppsUser, AppsUserModel>()
                .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            List<AppsUserModel> model = AutoMapper.Mapper.Map<List<AppsUserModel>>(dal.GetAll());

            return model;
        }

        public override AppsUserModel GetById(int id)
        {
            AppsUserDAL dal = new AppsUserDAL();
            AutoMapper.Mapper.CreateMap<AppsUser, AppsUserModel>();
            AutoMapper.Mapper.CreateMap<AppsUser, AppsUserModel>()
                .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));

            AppsUserModel model = AutoMapper.Mapper.Map<AppsUserModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(AppsUserModel obj)
        {
            AppsUserDAL dal = new AppsUserDAL();
            IAppsUser bl = new AppsUser();
            bl.Code = obj.Code;
            bl.Name = obj.Name;
            bl.Address = obj.Address;
            bl.PAN = obj.PAN;
            bl.DOB = obj.DOB;
            bl.Emailid = obj.Emailid;
            bl.AppLogin = obj.Emailid;
            bl.MobileNo = obj.MobileNo;
            bl.Status = 2;
            dal.InsertOrUpdate(bl);

        }

        public static void AddEmpRights(int id, string rightList)
        {
            AppsUserDAL dal = new AppsUserDAL();
            dal.AddRights(id, rightList);
        }

        public static int GetByUserName(string username)
        {
            return AppsUserDAL.GetByAppLogin(username).Id;
        }

        public static void DeleteRights(int empId, int RightId)
        {
            AppsUserDAL dal = new AppsUserDAL();
            IAppsUser bl = dal.GetById(empId);

            UserRightsDAL cdal = new UserRightsDAL();
            IUserRights rights = cdal.GetById(RightId);

            int i = bl.EmpRight.IndexOf(rights);
            bl.EmpRight.RemoveAt(i);
            dal.InsertOrUpdate(bl);
        }

        public static string[] LoginList()
        {
            AppsUserDAL dal = new AppsUserDAL();
            return dal.GetAll().Select(x => x.Emailid).ToArray();
        }

        //Assign company
        public static void AddAssignComp(int id, string compList)
        {
            AppsUserDAL dal = new AppsUserDAL();
            dal.AssignCompany(id, compList);
        }

        public static void DeleteCompany(int empId, int CompId)
        {
            AppsUserDAL dal = new AppsUserDAL();
            IAppsUser bl = dal.GetById(empId);

            CompanyDAL cdal = new CompanyDAL();
            ICompany comp = cdal.GetById(CompId);

            int i = bl.AssignCompany.IndexOf(comp);
            bl.AssignCompany.RemoveAt(i);
            dal.InsertOrUpdate(bl);
        }

    }
}