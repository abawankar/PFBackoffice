using DAL.Master;
using Domain.Implementation;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.Models;

namespace UI.Models.Master
{
    public class CompanyModel : Company
    {
        public int StatutoryCodeId { get; set; }
    }

    public class CompanyRepository : RepositoryModel<CompanyModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(CompanyModel obj)
        {
            CompanyDAL dal = new CompanyDAL();
            ICompany bl = dal.GetById(obj.Id);
            bl.Code = obj.Code;
            bl.Name = obj.Name;
            bl.Address = obj.Address;
            bl.StateCode = obj.StateCode;
            bl.PAN = obj.PAN;
            bl.GST = obj.GST;
            bl.ServiceTax = obj.ServiceTax;
            bl.Emailid = obj.Emailid;
            bl.PhoneNo = obj.PhoneNo;
            bl.CIN = obj.CIN;
            if (obj.RegDate != null)
                bl.RegDate = Convert.ToDateTime(obj.RegDate) + DateTime.Now.TimeOfDay;
            bl.RegNumber = obj.RegNumber;
            bl.WebSite = obj.WebSite;
            bl.ESIRegistrationNumber = obj.ESIRegistrationNumber;
            bl.EstablishmentCode = obj.EstablishmentCode;
            bl.StatutoryCode = new StatutoryCode { Id = obj.StatutoryCodeId };

            dal.InsertOrUpdate(bl);
        }

        public override IList<CompanyModel> GetAll()
        {
            CompanyDAL dal = new CompanyDAL();
            AutoMapper.Mapper.CreateMap<Company, CompanyModel>();
            AutoMapper.Mapper.CreateMap<Company, CompanyModel>()
               .ForMember(dest => dest.StatutoryCodeId, opt => opt.MapFrom(scr => scr.StatutoryCode.Id));
            List<CompanyModel> model = AutoMapper.Mapper.Map<List<CompanyModel>>(dal.GetAll());

            return model;
        }

        public IList<CompanyModel> GetAll(string username)
        {
            CompanyDAL dal = new CompanyDAL();
            AutoMapper.Mapper.CreateMap<Company, CompanyModel>();
            AutoMapper.Mapper.CreateMap<Company, CompanyModel>()
               .ForMember(dest => dest.StatutoryCodeId, opt => opt.MapFrom(scr => scr.StatutoryCode.Id));
            List<CompanyModel> model = AutoMapper.Mapper.Map<List<CompanyModel>>(AppsUserDAL.GetByAppLogin(username).AssignCompany);

            return model;
        }

        public override CompanyModel GetById(int id)
        {
            CompanyDAL dal = new CompanyDAL();
            AutoMapper.Mapper.CreateMap<Company, CompanyModel>();
            AutoMapper.Mapper.CreateMap<Company, CompanyModel>()
               .ForMember(dest => dest.StatutoryCodeId, opt => opt.MapFrom(scr => scr.StatutoryCode.Id));
            CompanyModel model = AutoMapper.Mapper.Map<CompanyModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(CompanyModel obj)
        {
            CompanyDAL dal = new CompanyDAL();
            ICompany bl = new Company();
            bl.Code = obj.Code;
            bl.Name = obj.Name;
            bl.Address = obj.Address;
            bl.StateCode = obj.StateCode;
            bl.PAN = obj.PAN;
            bl.GST = obj.GST;
            bl.ServiceTax = obj.ServiceTax;
            bl.Emailid = obj.Emailid;
            bl.PhoneNo = obj.PhoneNo;
            bl.CIN = obj.CIN;
            if (obj.RegDate != null)
                bl.RegDate = Convert.ToDateTime(obj.RegDate) + DateTime.Now.TimeOfDay;
            bl.RegNumber = obj.RegNumber;
            bl.WebSite = obj.WebSite;
            bl.ESIRegistrationNumber = obj.ESIRegistrationNumber;
            bl.EstablishmentCode = obj.EstablishmentCode;
            bl.StatutoryCode = new StatutoryCode { Id = obj.StatutoryCodeId };
            dal.InsertOrUpdate(bl);
        }
    }
}