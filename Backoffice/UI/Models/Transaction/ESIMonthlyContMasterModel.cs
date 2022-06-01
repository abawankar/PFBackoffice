using DAL.Transaction;
using Domain.Implementation.Transaction;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Transaction
{
    public class ESIFooterModel
    {
        public int ESIMember { get; set; }
        public double TotalIP { get; set; }
        public double TotalER { get; set; }
        public double TotalCont { get; set; }
        public double TotalGovtCont { get; set; }
        public double TotalWages { get; set; }
    }
    public class ESIMonthlyContMasterModel: Domain.Implementation.Transaction.ESIMonthlyContMaster
    {
        public int CompId { get; set; }
        public int ESIMember
        {
            get
            {
                return MonthlyCont != null ? MonthlyCont.Count() : 0;
            }
        }
        public double TotalIP
        {
            get
            {
                return MonthlyCont != null ? MonthlyCont.Sum(x=>x.IPCont) : 0;
            }
        }
        public double TotalCont
        {
            get
            {
                return MonthlyCont != null ? TotalIP+ERCont : 0;
            }
        }
        public double TotalWages
        {
            get
            {
                return MonthlyCont != null ? MonthlyCont.Sum(x => x.GrossWages) : 0;
            }
        }
    }

    public class ESIMonthlyContMasterRepository : RepositoryModel<ESIMonthlyContMasterModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(ESIMonthlyContMasterModel obj)
        {
            ESIMonthlyContMasterDAL dal = new ESIMonthlyContMasterDAL();
            IESIMonthlyContMaster bl = dal.GetById(obj.Id);
            bl.ChallanNo = obj.ChallanNo;
            bl.Date = obj.Date;
            dal.InsertOrUpdate(bl);
        }

        public override IList<ESIMonthlyContMasterModel> GetAll()
        {
            ESIMonthlyContMasterDAL dal = new ESIMonthlyContMasterDAL();
            AutoMapper.Mapper.CreateMap<ESIMonthlyContMaster, ESIMonthlyContMasterModel>();
            AutoMapper.Mapper.CreateMap<ESIMonthlyContMaster, ESIMonthlyContMasterModel>()
             .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            List<ESIMonthlyContMasterModel> model = AutoMapper.Mapper.Map<List<ESIMonthlyContMasterModel>>(dal.GetAll());

            return model;
        }

        public IList<ESIMonthlyContMasterModel> GetByCompId(int compId)
        {
            ESIMonthlyContMasterDAL dal = new ESIMonthlyContMasterDAL();
            AutoMapper.Mapper.CreateMap<ESIMonthlyContMaster, ESIMonthlyContMasterModel>();
            AutoMapper.Mapper.CreateMap<ESIMonthlyContMaster, ESIMonthlyContMasterModel>()
             .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            List<ESIMonthlyContMasterModel> model = AutoMapper.Mapper.Map<List<ESIMonthlyContMasterModel>>(dal.GetByCompId(compId));

            return model;
        }

        public override ESIMonthlyContMasterModel GetById(int id)
        {
            ESIMonthlyContMasterDAL dal = new ESIMonthlyContMasterDAL();
            AutoMapper.Mapper.CreateMap<ESIMonthlyContMaster, ESIMonthlyContMasterModel>();
            AutoMapper.Mapper.CreateMap<ESIMonthlyContMaster, ESIMonthlyContMasterModel>()
             .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            ESIMonthlyContMasterModel model = AutoMapper.Mapper.Map<ESIMonthlyContMasterModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(ESIMonthlyContMasterModel obj)
        {
            throw new NotImplementedException();
        }

        public IList<ESIMonthlyContModel> GetFullDetails(int id)
        {
            ESIMonthlyContMasterDAL dal = new ESIMonthlyContMasterDAL();
            AutoMapper.Mapper.CreateMap<ESIMonthlyCont, ESIMonthlyContModel>();
            List<ESIMonthlyContModel> model = AutoMapper.Mapper.Map<List<ESIMonthlyContModel>>(dal.GetById(id).MonthlyCont.ToList());

            return model;
        }
    }
}