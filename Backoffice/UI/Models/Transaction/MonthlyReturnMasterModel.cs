using DAL.Transaction;
using Domain.Implementation;
using Domain.Implementation.Transaction;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Transaction
{
    public class MonthlyReturnMasterModel:Domain.Implementation.Transaction.MonthlyReturnMaster
    {
        public int CompId { get; set; }
        public int ExemptedEmp
        { get {

                return MonthlyReturn!=null?MonthlyReturn.Where(x => x.UAN == "").Count():0;
            } }
        public int EPFEmp
        {
            get
            {

                return MonthlyReturn != null ? MonthlyReturn.Count()-ExemptedEmp : 0;
            }
        }
        public int EPSEmp
        {
            get
            {

                return MonthlyReturn != null ?EPFEmp- MonthlyReturn.Where(x=>x.EPSWages ==0).Count() + ExemptedEmp : 0;
            }
        }
        public double ExemptedGrossWages
        {
            get
            {
                return MonthlyReturn != null ? MonthlyReturn.Where(x => x.UAN == "").Sum(x => x.GrossWages):0;
            }
        }
        public double ExemptedWages
        {
            get
            {
                return MonthlyReturn != null ? MonthlyReturn.Where(x => x.UAN == "").Sum(x => x.EPFWages) : 0;
            }
        }
        public double EPFGrossWages
        {
            get
            {
                return MonthlyReturn != null ? MonthlyReturn.Sum(x => x.GrossWages)-ExemptedGrossWages : 0;
            }
        }
        public double EPFWages
        {
            get
            {
                return MonthlyReturn != null ? MonthlyReturn.Sum(x => x.EPFWages)-ExemptedWages:0;
            }
        }
        public double EPSWages
        {
            get
            {
                return MonthlyReturn != null ? MonthlyReturn.Sum(x => x.EPSWages):0;
            }
        }
        public double EDLIWages
        {
            get
            {
                return MonthlyReturn != null ? MonthlyReturn.Sum(x => x.EDLIWages) : 0;
            }
        }
        public double EECont
        {
            get
            {
                return MonthlyReturn != null ? MonthlyReturn.Sum(x => x.EECont):0;
            }
        }
        public double EPSCont
        {
            get
            {
                return MonthlyReturn != null ? MonthlyReturn.Sum(x => x.EPSCont):0;
            }
        }
        public double ERCont
        {
            get
            {
                return MonthlyReturn != null ? MonthlyReturn.Sum(x => x.ERCont):0;
            }
        }
        public double NCPDays
        {
            get
            {
                return MonthlyReturn != null ? MonthlyReturn.Sum(x => x.NCPDays):0;
            }
        }
        public double Ac2
        {
            get
            {
                return MonthlyReturn != null ? (EPFWages * AdminAct2) / 100 : 0;
            }
        }
        public double Ac21
        {
            get
            {
                return MonthlyReturn != null ? (EDLIWages * EDLIAct21) / 100 : 0;
            }
        }
        public double Total
        {
            get
            {
                return EECont + EPSCont + ERCont + Ac2 + Ac21;
            }
        }
        public double ESIWages
        {
            get
            {
                return MonthlyReturn != null ? MonthlyReturn.Sum(x => x.ESIWages) : 0;
            }
        }

        public double EditAc21 { get; set; }
    }

    public class MonthlyReturnMasterRepository : RepositoryModel<MonthlyReturnMasterModel>
    {
        public override bool Delete(int id)
        {
            MonthlyReturnMasterDAL dal = new MonthlyReturnMasterDAL();
            IMonthlyReturnMaster bl = dal.GetById(id);
            return dal.Delete(bl);
        }

        public override void Edit(MonthlyReturnMasterModel obj)
        {
            MonthlyReturnMasterDAL dal = new MonthlyReturnMasterDAL();
            IMonthlyReturnMaster bl = dal.GetById(obj.Id);
            bl.TRRN = obj.TRRN;
            bl.CRN = obj.CRN;
            bl.PaymentDate = obj.PaymentDate;
            if(obj.EditAc21 != 0)
                bl.EDLIAct21 = (obj.EditAc21 / (bl.MonthlyReturn.Sum(x => x.EDLIWages))) * 100;
            dal.InsertOrUpdate(bl);
        }

        public override IList<MonthlyReturnMasterModel> GetAll()
        {
            MonthlyReturnMasterDAL dal = new MonthlyReturnMasterDAL();
            AutoMapper.Mapper.CreateMap<MonthlyReturnMaster, MonthlyReturnMasterModel>();
            AutoMapper.Mapper.CreateMap<MonthlyReturnMaster, MonthlyReturnMasterModel>()
             .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            List<MonthlyReturnMasterModel> model = AutoMapper.Mapper.Map<List<MonthlyReturnMasterModel>>(dal.GetAll());

            return model;
        }

        public IList<MonthlyReturnMasterModel> NotPaid()
        {
            MonthlyReturnMasterDAL dal = new MonthlyReturnMasterDAL();
            AutoMapper.Mapper.CreateMap<MonthlyReturnMaster, MonthlyReturnMasterModel>();
            AutoMapper.Mapper.CreateMap<MonthlyReturnMaster, MonthlyReturnMasterModel>()
             .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            List<MonthlyReturnMasterModel> model = AutoMapper.Mapper.Map<List<MonthlyReturnMasterModel>>(dal.NotPaid());

            return model;
        }

        public IList<MonthlyReturnMasterModel> GetByCompId(int compid)
        {
            MonthlyReturnMasterDAL dal = new MonthlyReturnMasterDAL();
            AutoMapper.Mapper.CreateMap<MonthlyReturnMaster, MonthlyReturnMasterModel>();
            AutoMapper.Mapper.CreateMap<MonthlyReturnMaster, MonthlyReturnMasterModel>()
             .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            List<MonthlyReturnMasterModel> model = AutoMapper.Mapper.Map<List<MonthlyReturnMasterModel>>(dal.GetByCompId(compid));

            return model;
        }

        public override MonthlyReturnMasterModel GetById(int id)
        {
            MonthlyReturnMasterDAL dal = new MonthlyReturnMasterDAL();
            AutoMapper.Mapper.CreateMap<MonthlyReturnMaster, MonthlyReturnMasterModel>();
            AutoMapper.Mapper.CreateMap<MonthlyReturnMaster, MonthlyReturnMasterModel>()
             .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            MonthlyReturnMasterModel model = AutoMapper.Mapper.Map<MonthlyReturnMasterModel>(dal.GetById(id));

            return model;
        }

        public  MonthlyReturnMasterModel GetById(string month, int id, string ecr)
        {
            MonthlyReturnMasterDAL dal = new MonthlyReturnMasterDAL();
            AutoMapper.Mapper.CreateMap<MonthlyReturnMaster, MonthlyReturnMasterModel>();
            AutoMapper.Mapper.CreateMap<MonthlyReturnMaster, MonthlyReturnMasterModel>()
             .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            MonthlyReturnMasterModel model = AutoMapper.Mapper.Map<MonthlyReturnMasterModel>(dal.GetByCompIdMonthEcr(month,id,ecr));

            return model;
        }

        public override void Insert(MonthlyReturnMasterModel obj)
        {
            MonthlyReturnMasterDAL dal = new MonthlyReturnMasterDAL();
            IMonthlyReturnMaster bl = new MonthlyReturnMaster();
            bl.ContType = obj.ContType;
            bl.MonthYear = obj.MonthYear;
            bl.Company = new Company { Id = obj.CompId };
            bl.MonthlyReturn = obj.MonthlyReturn;
            dal.InsertOrUpdate(bl);
        }

        public IList<MonthlyReturnModel> GetFullDetails(int id)
        {
            MonthlyReturnMasterDAL dal = new MonthlyReturnMasterDAL();
            AutoMapper.Mapper.CreateMap<MonthlyReturn, MonthlyReturnModel>();
            List<MonthlyReturnModel> model = AutoMapper.Mapper.Map<List<MonthlyReturnModel>>(dal.GetById(id).MonthlyReturn.ToList());

            return model;
        }
    }

}