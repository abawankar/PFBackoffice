using DAL.Master;
using Domain.Implementation;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Master
{
    public class EmployeeModel : Domain.Implementation.Employee
    {
        public int CompId { get; set; }
        public string Aadhaar {
            get {
                if(KYC != null)
                {
                    foreach (var item in KYC)
                    {
                        if (item.DoxType == "A")
                            return item.DocumentNumber;
                    }
                }
                return "";
            }
        }
        public string PAN
        {
            get
            {
                if (KYC != null)
                {
                    foreach (var item in KYC)
                    {
                        if (item.DoxType == "P")
                            return item.DocumentNumber;
                    }
                }
                return "";
            }
        }
        public string BankAcc
        {
            get
            {
                if (KYC != null)
                {
                    foreach (var item in KYC)
                    {
                        if (item.DoxType == "B")
                            return item.DocumentNumber;
                    }
                }
                return "";
            }
        }
        public string Aadharno { get; set; }

    }

    public class PMRYModel
    {
        public int Id { get; set; }
        public string CompName { get; set; }
        public string EmpName { get; set; }
        public string UAN { get; set; }
        public string Aaadhaar { get; set; }
        public double GrossWages { get; set; }
        public string JobDescription { get; set; }
        public DateTime? DOJ { get; set; }
    }

    public class EmployeeRepository : RepositoryModel<EmployeeModel>
    {
        public override bool Delete(int id)
        {
            EmployeeDAL dal = new EmployeeDAL();
            IEmployee emp = dal.GetById(id);
            return dal.Delete(emp);
        }

        public override void Edit(EmployeeModel obj)
        {
            EmployeeDAL dal = new EmployeeDAL();
            IEmployee bl = dal.GetById(obj.Id);
            bl.UAN = obj.UAN;
            bl.MemberId = obj.MemberId;
            bl.EmpCode = obj.EmpCode;
            bl.Name = obj.Name;
            bl.Gender = obj.Gender;
            bl.ForHName = obj.ForHName;
            bl.ForHFlag = obj.ForHFlag;
            bl.DOB = obj.DOB;
            bl.DOJ = obj.DOJ;
            bl.Status = 1;
            if (obj.DOE != null)
            {
                bl.DOE = obj.DOE;
                bl.Status = 2;
            }
                
            bl.Nationality = obj.Nationality;
            bl.MaritalStatus = obj.MaritalStatus;
            bl.Mobile = obj.Mobile;
            bl.EmailId = obj.EmailId;
            bl.ReasonForLeaving = obj.ReasonForLeaving;
            bl.CellingEPF = obj.CellingEPF;
            bl.CellingEPS = obj.CellingEPS;
            bl.VPF = obj.VPF;
            bl.PFExempted = obj.PFExempted;
            bl.ESIExempted = obj.ESIExempted;
            bl.ESIIP = obj.ESIIP;
            bl.Branch = obj.Branch;
            dal.InsertOrUpdate(bl);
        }
        public void EditESI(EmployeeModel obj)
        {
            EmployeeDAL dal = new EmployeeDAL();
            IEmployee bl = dal.GetById(obj.Id);
            bl.ESIIP = obj.ESIIP;
            bl.IPStatus = obj.IPStatus;
            dal.InsertOrUpdate(bl);
        }

        public override IList<EmployeeModel> GetAll()
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>()
               .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            List<EmployeeModel> model = AutoMapper.Mapper.Map<List<EmployeeModel>>(dal.GetAll());

            return model;
        }

        public IList<EmployeeModel> GetByCompId(int compid)
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>()
               .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            List<EmployeeModel> model = AutoMapper.Mapper.Map<List<EmployeeModel>>(dal.GetByCompId(compid));

            return model;
        }

        public IList<EmployeeModel> GetESIByCompId(int compid)
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>()
               .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            List<EmployeeModel> model = AutoMapper.Mapper.Map<List<EmployeeModel>>(dal.GetESIByCompId(compid));

            return model;
        }

        public IList<EmployeeModel> GetByCompId(int compid, string esi)
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>()
               .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            List<EmployeeModel> model = AutoMapper.Mapper.Map<List<EmployeeModel>>(dal.GetByCompId(compid,esi));

            return model;
        }

        public override EmployeeModel GetById(int id)
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>()
               .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            EmployeeModel model = AutoMapper.Mapper.Map<EmployeeModel>(dal.GetById(id));

            return model;
        }

        public EmployeeModel GetByUAN(string uan)
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>()
               .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            EmployeeModel model = AutoMapper.Mapper.Map<EmployeeModel>(dal.GetByUAN(uan));

            return model;
        }

        public EmployeeModel GetByCompAndCode(int compid, string empcode)
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>()
               .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            EmployeeModel model = AutoMapper.Mapper.Map<EmployeeModel>(dal.GetByCompAndCode(compid,empcode));

            return model;
        }

        public List<EmployeeModel> GetByESICode(string empcode)
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>()
               .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            List<EmployeeModel> model = AutoMapper.Mapper.Map<List<EmployeeModel>>(dal.GetByESICode(empcode));

            return model;
        }

        public override void Insert(EmployeeModel obj)
        {
            EmployeeDAL dal = new EmployeeDAL();
            IEmployee bl = new Employee();
            bl.UAN = obj.UAN;
            bl.MemberId = obj.MemberId;
            bl.EmpCode = obj.EmpCode;
            bl.Name = obj.Name;
            bl.Gender = obj.Gender;
            bl.ForHName = obj.ForHName;
            bl.ForHFlag = obj.ForHFlag;
            bl.DOB = obj.DOB;
            bl.DOJ = obj.DOJ;
            bl.DOE = obj.DOE;
            bl.Nationality = obj.Nationality;
            bl.MaritalStatus = obj.MaritalStatus;
            bl.Mobile = obj.Mobile;
            bl.EmailId = obj.EmailId;
            bl.ReasonForLeaving = obj.ReasonForLeaving;
            bl.CellingEPF = obj.CellingEPF;
            bl.CellingEPS = obj.CellingEPS;
            bl.Company = new Company { Id = obj.CompId };
            bl.Status = 1;
            bl.VPF = obj.VPF;
            bl.PFExempted = obj.PFExempted;
            bl.ESIExempted = obj.ESIExempted;
            dal.InsertOrUpdate(bl);
        }

        public void Insert(EmployeeModel obj, int compid)
        {
            EmployeeDAL dal = new EmployeeDAL();
            IEmployee bl = new Employee();
            bl.UAN = obj.UAN;
            bl.Name = obj.Name;
            bl.MemberId = obj.MemberId;
            bl.EmpCode = obj.EmpCode;
            bl.Gender = obj.Gender;
            bl.ForHName = obj.ForHName;
            bl.ForHFlag = obj.ForHFlag;
            bl.Nationality = obj.Nationality;
            bl.MaritalStatus = obj.MaritalStatus;
            bl.DOB = obj.DOB;
            bl.DOJ = obj.DOJ;
            bl.DOE = obj.DOE;
            bl.ReasonForLeaving = obj.ReasonForLeaving;
            bl.CellingEPF = obj.CellingEPF;
            bl.CellingEPS = obj.CellingEPS;
            bl.Company = new Company { Id = compid };
            bl.VPF = obj.VPF;
            bl.PFExempted = obj.PFExempted;
            bl.ESIExempted = obj.ESIExempted;
            bl.ESIIP = obj.ESIIP;
            bl.Status = 1;
            bl.VPF = obj.VPF;
            bl.Branch = obj.Branch;
            dal.InsertOrUpdate(bl);
        }
    }
}