using DAL.Master;
using Domain.Implementation;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Billing
{
    public class ServiceNameModel : Domain.Implementation.ServiceName
    {
    }

    public class ServiceNameRepository : RepositoryModel<ServiceNameModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(ServiceNameModel obj)
        {
            ServiceNameDAL dal = new ServiceNameDAL();
            IServiceName bl = dal.GetById(obj.Id);
            bl.Name = obj.Name;
            bl.SAC = obj.SAC;
            bl.Rate = obj.Rate;
            dal.InsertOrUpdate(bl);
        }

        public override IList<ServiceNameModel> GetAll()
        {
            ServiceNameDAL dal = new ServiceNameDAL();
            AutoMapper.Mapper.CreateMap<ServiceName, ServiceNameModel>();
            List<ServiceNameModel> model = AutoMapper.Mapper.Map<List<ServiceNameModel>>(dal.GetAll());
            return model;
        }

        public override ServiceNameModel GetById(int id)
        {
            ServiceNameDAL dal = new ServiceNameDAL();
            AutoMapper.Mapper.CreateMap<ServiceName, ServiceNameModel>();
            ServiceNameModel model = AutoMapper.Mapper.Map<ServiceNameModel>(dal.GetById(id));
            return model;
        }

        public override void Insert(ServiceNameModel obj)
        {
            ServiceNameDAL dal = new ServiceNameDAL();
            IServiceName bl = new ServiceName();
            bl.Name = obj.Name;
            bl.SAC = obj.SAC;
            bl.Rate = obj.Rate;
            dal.InsertOrUpdate(bl);
        }
    }
}