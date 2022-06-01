using DAL.Transaction;
using Domain.Implementation.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.CMC
{
    public class CMCFooterModel
    {
        public string Month { get; set; }
        public int NoBank { get; set; }
        public int TotalMember { get; set; }
        public double Honorarium { get; set; }
        public double Contingency { get; set; }
        public double TAOPE { get; set; }
        public double TotalPayable { get; set; }
        public double Review { get; set; }
        public double Meeting { get; set; }
        public double Claim { get; set; }
        public double NetPayable { get; set; }
    }

    public class CMCTrnMasterModel:Domain.Implementation.Transaction.CMCTrnMaster
    {
        public double TotalMember
        {
            get
            {
                return Trn != null ? Trn.Count() : 0;
            }
        }
        public double Honorarium
        {
            get
            {
                return Trn != null ? Trn.Sum(x => x.Honorarium) : 0;
            }
        }
        public double Contingency
        {
            get
            {
                return Trn != null ? Trn.Sum(x => x.Contingency) : 0;
            }
        }
        public double TAOPE
        {
            get
            {
                return Trn != null ? Trn.Sum(x => x.TAOPE) : 0;
            }
        }
        public double TotalPayable
        {
            get
            {
                return Trn != null ? Trn.Sum(x => x.Honorarium+x.Contingency+x.TAOPE) : 0;
            }
        }
        public double Review
        {
            get
            {
                return Trn != null ? Trn.Sum(x => x.Review) : 0;
            }
        }
        public double Meeting
        {
            get
            {
                return Trn != null ? Trn.Sum(x => x.Meeting) : 0;
            }
        }
        public double Claim
        {
            get
            {
                return Trn != null ? Trn.Sum(x => x.Claim) : 0;
            }
        }
        public double NetPayable
        {
            get
            {
                return Trn != null ? Trn.Sum(x => x.Review + x.Meeting + x.Claim)+TotalPayable : 0;
            }
        }
    }

    public class CMCTrnMasterRepository : RepositoryModel<CMCTrnMasterModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(CMCTrnMasterModel obj)
        {
            throw new NotImplementedException();
        }

        public override IList<CMCTrnMasterModel> GetAll()
        {

            CMCTrnMasterDAL dal = new CMCTrnMasterDAL();
            AutoMapper.Mapper.CreateMap<CMCTrnMaster, CMCTrnMasterModel>();
            List<CMCTrnMasterModel> model = AutoMapper.Mapper.Map<List<CMCTrnMasterModel>>(dal.GetAll());

            return model;
        }

        public override CMCTrnMasterModel GetById(int id)
        {
            CMCTrnMasterDAL dal = new CMCTrnMasterDAL();
            AutoMapper.Mapper.CreateMap<CMCTrnMaster, CMCTrnMasterModel>();
            CMCTrnMasterModel model = AutoMapper.Mapper.Map<CMCTrnMasterModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(CMCTrnMasterModel obj)
        {
            throw new NotImplementedException();
        }

        public IList<CMCTransactionModel> GetFullDetails(int id)
        {
            CMCTrnMasterDAL dal = new CMCTrnMasterDAL();
            AutoMapper.Mapper.CreateMap<CMCTransaction, CMCTransactionModel>();
            AutoMapper.Mapper.CreateMap<CMCTransaction, CMCTransactionModel>()
               .ForMember(dest => dest.MemberId, opt => opt.MapFrom(scr => scr.Member.Id))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(scr => scr.Member.Name))
              .ForMember(dest => dest.Account, opt => opt.MapFrom(scr => scr.Member.BankAc))
              .ForMember(dest => dest.Code, opt => opt.MapFrom(scr => scr.Member.Code));
            List<CMCTransactionModel> model = AutoMapper.Mapper.Map<List<CMCTransactionModel>>(dal.GetById(id).Trn.ToList());

            return model;
        }
    }
}