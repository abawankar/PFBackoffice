using DAL.Master;
using Domain.Implementation;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Master
{
    public class UserRightsModel : UserRights
    {

    }

    public class UserRightsRepository : RepositoryModel<UserRightsModel>
    {

        public override UserRightsModel GetById(int id)
        {
            UserRightsDAL dal = new UserRightsDAL();
            AutoMapper.Mapper.CreateMap<UserRights, UserRightsModel>();
            UserRightsModel model = AutoMapper.Mapper.Map<UserRightsModel>(dal.GetById(id));

            return model;
        }

        public override IList<UserRightsModel> GetAll()
        {
            UserRightsDAL dal = new UserRightsDAL();
            AutoMapper.Mapper.CreateMap<UserRights, UserRightsModel>();
            List<UserRightsModel> model = AutoMapper.Mapper.Map<List<UserRightsModel>>(dal.GetAll());

            return model;
        }

        public override void Edit(UserRightsModel obj)
        {
            UserRightsDAL dal = new UserRightsDAL();
            IUserRights bl = dal.GetById(obj.Id);
            bl.Code = obj.Code;
            bl.MnuName = obj.MnuName;
            bl.TableName = obj.TableName;
            bl.Operation = obj.Operation;
            bl.Description = obj.Description;
            dal.InsertOrUpdate(bl);
        }

        public override void Insert(UserRightsModel obj)
        {
            UserRightsDAL dal = new UserRightsDAL();
            IUserRights bl = new UserRights();
            bl.Code = obj.Code;
            bl.MnuName = obj.MnuName;
            bl.TableName = obj.TableName;
            bl.Operation = obj.Operation;
            bl.Description = obj.Description;

            dal.InsertOrUpdate(bl);
        }

        public override bool Delete(int id)
        {
            UserRightsDAL dal = new UserRightsDAL();
            IUserRights bl = dal.GetById(id);
            return dal.Delete(bl);
        }

        public static string[] RightList(string username)
        {
            var data = AppsUserDAL.GetByAppLogin(username.Trim()).EmpRight.Select(x => x.Code).ToArray();
            return data;
        }
    }
}