using DAL.Master;
using Domain.Implementation;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Master
{
    public class EmployeeKYCModel : Domain.Implementation.EmployeeKYC
    {
        public string EmpCode { get; set; }
        public string UAN { get; set; }
        public int Compid { get; set; }
    }

    public class EmployeeKYCRepository : RepositoryModel<EmployeeKYCModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(EmployeeKYCModel obj)
        {
            EmployeeKYCDAL dal = new EmployeeKYCDAL();
            IEmployeeKYC bl = dal.GetById(obj.Id);
            bl.DocumentNumber = obj.DocumentNumber;
            bl.NameonDox = obj.NameonDox;
            bl.Other = obj.Other;
            bl.DoxType = obj.DoxType;
            bl.IssueDate = obj.IssueDate;
            bl.Exipiry = obj.Exipiry;
            bl.Place = obj.Place;
            dal.InsertOrUpdate(bl);
        }

        public override IList<EmployeeKYCModel> GetAll()
        {
            EmployeeKYCDAL dal = new EmployeeKYCDAL();
            AutoMapper.Mapper.CreateMap<EmployeeKYC, EmployeeKYCModel>();
            List<EmployeeKYCModel> model = AutoMapper.Mapper.Map<List<EmployeeKYCModel>>(dal.GetAll());

            return model;
        }

        public IList<EmployeeKYCModel> GetAll(int empid)
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<EmployeeKYC, EmployeeKYCModel>();
            List<EmployeeKYCModel> model = AutoMapper.Mapper.Map<List<EmployeeKYCModel>>(dal.GetById(empid).KYC.ToList());

            return model;
        }

        public override EmployeeKYCModel GetById(int id)
        {
            EmployeeKYCDAL dal = new EmployeeKYCDAL();
            AutoMapper.Mapper.CreateMap<EmployeeKYC, EmployeeKYCModel>();
            EmployeeKYCModel model = AutoMapper.Mapper.Map<EmployeeKYCModel>(dal.GetById(id));

            return model;
        }


        public override void Insert(EmployeeKYCModel obj)
        {
            throw new NotImplementedException();
        }

        public void Insert(EmployeeKYCModel obj,int empid)
        {
            EmployeeDAL dal = new EmployeeDAL();
            IEmployeeKYC bl = new EmployeeKYC();
            bl.DocumentNumber = obj.DocumentNumber;
            bl.NameonDox = obj.NameonDox;
            bl.Other = obj.Other;
            bl.DoxType = obj.DoxType;
            bl.IssueDate = obj.IssueDate;
            bl.Exipiry = obj.Exipiry;
            bl.Place = obj.Place;
            IEmployee emp = dal.GetById(empid);
            emp.KYC.Add(bl);
            dal.InsertOrUpdate(emp);
        }
    }
}