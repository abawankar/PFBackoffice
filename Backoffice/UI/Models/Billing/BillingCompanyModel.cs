using DAL.Master;
using Domain.Implementation;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Billing
{
    public class BillingCompanyModel : Domain.Implementation.BillingCompany
    {
    }

    public class BillingCompanyRepository : RepositoryModel<BillingCompanyModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(BillingCompanyModel obj)
        {
            BillingCompanyDAL dal = new BillingCompanyDAL();
            IBillingCompany bl = dal.GetById(obj.Id);
            bl.Name = obj.Name;
            bl.Address = obj.Address;
            bl.GST = obj.GST;
            bl.PAN = obj.PAN;
            bl.StateCode = obj.StateCode;
            dal.InsertOrUpdate(bl);
        }

        public override IList<BillingCompanyModel> GetAll()
        {
            BillingCompanyDAL dal = new BillingCompanyDAL();
            AutoMapper.Mapper.CreateMap<BillingCompany, BillingCompanyModel>();
            List<BillingCompanyModel> model = AutoMapper.Mapper.Map<List<BillingCompanyModel>>(dal.GetAll());
            return model;
        }

        public override BillingCompanyModel GetById(int id)
        {
            BillingCompanyDAL dal = new BillingCompanyDAL();
            AutoMapper.Mapper.CreateMap<BillingCompany, BillingCompanyModel>();
            BillingCompanyModel model = AutoMapper.Mapper.Map<BillingCompanyModel>(dal.GetById(id));
            return model;
        }

        public override void Insert(BillingCompanyModel obj)
        {
            BillingCompanyDAL dal = new BillingCompanyDAL();
            IBillingCompany bl = new BillingCompany();
            bl.Name = obj.Name;
            bl.Address = obj.Address;
            bl.GST = obj.GST;
            bl.PAN = obj.PAN;
            bl.StateCode = obj.StateCode;
            dal.InsertOrUpdate(bl);
        }
    }
}