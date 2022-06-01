using DAL.Master;
using Domain.Implementation.Master;
using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Master
{
    public class ESIEmpModel:Domain.Implementation.Master.ESIEmp
    {
        public string CompId { get; set; }
    }

    public class ESIEmpRepository : RepositoryModel<ESIEmpModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(ESIEmpModel obj)
        {
            ESIEmpDAL dal = new ESIEmpDAL();
            IESIEmp bl = dal.GetById(obj.Id);
            //bl.CompName = obj.CompId;
            //bl.EmpCode = obj.EmpCode;
            //bl.Name = obj.Name;
            //bl.FName = obj.FName;
            //bl.DOB = obj.DOB;
            //bl.DOJ = obj.DOJ;
            //bl.Gender = obj.Gender;
            //bl.PAddress = obj.PAddress;
            //bl.PrAddress = obj.PrAddress;
            //bl.NName = obj.NName;
            //bl.NomRelation = obj.NomRelation;
            //bl.NomAddress = obj.NomAddress;
            //bl.OldIns = obj.OldIns;
            bl.Status = obj.Status;
            dal.InsertOrUpdate(bl);
        }

        public override IList<ESIEmpModel> GetAll()
        {
            ESIEmpDAL dal = new ESIEmpDAL();
            AutoMapper.Mapper.CreateMap<ESIEmp, ESIEmpModel>();
            List<ESIEmpModel> model = AutoMapper.Mapper.Map<List<ESIEmpModel>>(dal.GetAll());

            return model;
        }

        public override ESIEmpModel GetById(int id)
        {
            ESIEmpDAL dal = new ESIEmpDAL();
            AutoMapper.Mapper.CreateMap<ESIEmp, ESIEmpModel>();
            ESIEmpModel model = AutoMapper.Mapper.Map<ESIEmpModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(ESIEmpModel obj)
        {
            ESIEmpDAL dal = new ESIEmpDAL();
            IESIEmp bl = new ESIEmp();
            bl.CompName = obj.CompName;
            bl.EmpCode = obj.EmpCode;
            bl.Name = obj.Name;
            bl.FName = obj.FName;
            bl.DOB = obj.DOB;
            bl.DOJ = obj.DOJ;
            bl.Gender = obj.Gender;
            bl.PAddress = obj.PAddress;
            bl.PrAddress = obj.PrAddress;
            bl.NName = obj.NName;
            bl.NomRelation = obj.NomRelation;
            bl.NomAddress = obj.NomAddress;
            bl.OldIns = obj.OldIns;
            bl.Status = 0;
            dal.InsertOrUpdate(bl);
        }
    }
}