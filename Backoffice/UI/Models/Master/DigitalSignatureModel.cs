using DAL.Master;
using Domain.Implementation;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Master
{
    public class DigitalSignatureModel : Domain.Implementation.DigitalSignature
    {
        public string CompName { get; set; }
        public double Days { get; set; }
    }

    public class DigitalSignatureRepository : RepositoryModel<DigitalSignatureModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(DigitalSignatureModel obj)
        {
            DigitalSignatureDAL dal = new DigitalSignatureDAL();
            IDigitalSignature bl = dal.GetById(obj.Id);
            bl.Name = obj.Name;
            bl.Validity = obj.Validity;
            dal.InsertOrUpdate(bl);
        }

        public override IList<DigitalSignatureModel> GetAll()
        {
            DigitalSignatureDAL dal = new DigitalSignatureDAL();
            AutoMapper.Mapper.CreateMap<DigitalSignature, DigitalSignatureModel>();
            List<DigitalSignatureModel> model = AutoMapper.Mapper.Map<List<DigitalSignatureModel>>(dal.GetAll());

            return model;
        }

        public IList<DigitalSignatureModel> GetAll(int compid)
        {
            CompanyDAL dal = new CompanyDAL();
            AutoMapper.Mapper.CreateMap<DigitalSignature, DigitalSignatureModel>();
            List<DigitalSignatureModel> model = AutoMapper.Mapper.Map<List<DigitalSignatureModel>>(dal.GetById(compid).DigitalSign.ToList());

            return model;
        }

        public override DigitalSignatureModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Insert(DigitalSignatureModel obj)
        {
            throw new NotImplementedException();
        }

        public void Insert(DigitalSignatureModel obj, int compid)
        {
            CompanyDAL dal = new CompanyDAL();
            ICompany comp = dal.GetById(compid);
            IDigitalSignature bl = new DigitalSignature();
            bl.Name = obj.Name;
            bl.Validity = obj.Validity;
            comp.DigitalSign.Add(bl);
            dal.InsertOrUpdate(comp);
        }
    }
}