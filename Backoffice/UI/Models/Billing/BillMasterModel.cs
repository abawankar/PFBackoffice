using DAL.Transaction;
using Domain.Implementation.Transaction;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Billing
{
    public class BillMasterModel:Domain.Implementation.Transaction.BillMaster
    {
        public int CompId { get; set; }
        public int BillingCompId { get; set; }

        public double InvAmt { get {
                return Tran != null ? Tran.Sum(x => x.Amount) : 0;
            } }

        public double CGST
        {
            get
            {
                return Tran != null ? Tran.Sum(x => x.CGST) : 0;
            }
        }

        public double SGST
        {
            get
            {
                return Tran != null ? Tran.Sum(x => x.SGST) : 0;
            }
        }

        public double IGST
        {
            get
            {
                return Tran != null ? Tran.Sum(x => x.IGST) : 0;
            }
        }

        public double GSTAmount
        {
            get
            {
                return Tran != null ? Tran.Sum(x => x.CGST+x.SGST+x.IGST) : 0;
            }
        }
        public double GrandTotal
        {
            get
            {
                return InvAmt + GSTAmount;
            }
        }
    }

    public class BillMasterRepository : RepositoryModel<BillMasterModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(BillMasterModel obj)
        {
            BillMasterDAL dal = new BillMasterDAL();
            IBillMaster bl = dal.GetById(obj.Id);
            bl.Status = obj.Status;
            dal.InsertOrUpdate(bl);
        }

        public override IList<BillMasterModel> GetAll()
        {
            BillMasterDAL dal = new BillMasterDAL();
            AutoMapper.Mapper.CreateMap<BillMaster, BillMasterModel>();
            AutoMapper.Mapper.CreateMap<BillMaster, BillMasterModel>()
               .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id))
               .ForMember(dest => dest.BillingCompId, opt => opt.MapFrom(scr => scr.BillingCompany.Id));
            List<BillMasterModel> model = AutoMapper.Mapper.Map<List<BillMasterModel>>(dal.GetAll());

            return model;
        }

        public IList<BillTransactionModel> BillTransaction(int billid)
        {
            BillMasterDAL dal = new BillMasterDAL();
            AutoMapper.Mapper.CreateMap<BillTransaction, BillTransactionModel>();
            AutoMapper.Mapper.CreateMap<BillTransaction, BillTransactionModel>()
               .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(scr => scr.Service.Id));
            List<BillTransactionModel> model = AutoMapper.Mapper.Map<List<BillTransactionModel>>(dal.GetById(billid).Tran.ToList());
            return model;
        }

        public override BillMasterModel GetById(int id)
        {
            BillMasterDAL dal = new BillMasterDAL();
            AutoMapper.Mapper.CreateMap<BillMaster, BillMasterModel>();
            AutoMapper.Mapper.CreateMap<BillMaster, BillMasterModel>()
               .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id))
               .ForMember(dest => dest.BillingCompId, opt => opt.MapFrom(scr => scr.BillingCompany.Id));
            BillMasterModel model = AutoMapper.Mapper.Map<BillMasterModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(BillMasterModel obj)
        {
            throw new NotImplementedException();
        }
    }

}