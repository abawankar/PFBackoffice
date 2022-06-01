using DAL.Transaction;
using Domain.Implementation.Transaction;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Transaction
{
    public class MonthlyReturnModel : Domain.Implementation.Transaction.MonthlyReturn
    {
        public int CompId { get; set; }
    }

    public class MonthlyReturnDraftModel : Domain.Implementation.Transaction.MonthlyReturnDraft
    {
        public int CompId { get; set; }
        public DateTime? DOL { get; set; }

    }

    public class MonthlyReturnRepository : RepositoryModel<MonthlyReturnModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(MonthlyReturnModel obj)
        {
            throw new NotImplementedException();
        }

        public override IList<MonthlyReturnModel> GetAll()
        {
            MonthlyReturnDAL dal = new MonthlyReturnDAL();
            AutoMapper.Mapper.CreateMap<MonthlyReturn, MonthlyReturnModel>();
            List<MonthlyReturnModel> model = AutoMapper.Mapper.Map<List<MonthlyReturnModel>>(dal.GetAll());

            return model;
        }

        public IList<MonthlyReturnModel> GetByCompIdMonthEcr(string month, int Compid, string ecr)
        {
            MonthlyReturnDAL dal = new MonthlyReturnDAL();
            AutoMapper.Mapper.CreateMap<MonthlyReturn, MonthlyReturnModel>();
            List<MonthlyReturnModel> model = AutoMapper.Mapper.Map<List<MonthlyReturnModel>>(dal.GetByCompIdMonthEcr(month, Compid, ecr));

            return model;
        }

        public IList<MonthlyReturnModel> GetByCompIdMonth(string month, int Compid)
        {
            MonthlyReturnMasterDAL dal = new MonthlyReturnMasterDAL();
            AutoMapper.Mapper.CreateMap<MonthlyReturn, MonthlyReturnModel>();
            List<MonthlyReturnModel> model = AutoMapper.Mapper.Map<List<MonthlyReturnModel>>(dal.GetByCompIdMonth(month,Compid).SelectMany(x=>x.MonthlyReturn).ToList());

            return model;
        }

        public override MonthlyReturnModel GetById(int id)
        {
            MonthlyReturnDAL dal = new MonthlyReturnDAL();
            AutoMapper.Mapper.CreateMap<MonthlyReturn, MonthlyReturnModel>();
            MonthlyReturnModel model = AutoMapper.Mapper.Map<MonthlyReturnModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(MonthlyReturnModel obj)
        {
            throw new NotImplementedException();
        }
    }

    public class MonthlyReturnDraftRepository : RepositoryModel<MonthlyReturnDraftModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(MonthlyReturnDraftModel obj)
        {
            MonthlyReturnDraftDAL dal = new MonthlyReturnDraftDAL();
            IMonthlyReturnDraft bl = dal.GetById(obj.Id);
            bl.GrossWages = obj.GrossWages;
            bl.EPFWages = obj.EPFWages;
            bl.EPSWages = obj.EPSWages;
            bl.EDLIWages = obj.EDLIWages;
            bl.EECont = obj.EECont;
            bl.EPSCont = obj.EPSCont;
            bl.ERCont = obj.ERCont;
            bl.NCPDays = obj.NCPDays;
            bl.ESIWages = obj.ESIWages;
            dal.InsertOrUpdate(bl);
        }

        public override IList<MonthlyReturnDraftModel> GetAll()
        {
            MonthlyReturnDraftDAL dal = new MonthlyReturnDraftDAL();
            AutoMapper.Mapper.CreateMap<MonthlyReturnDraft, MonthlyReturnDraftModel>();
            AutoMapper.Mapper.CreateMap<MonthlyReturnDraft, MonthlyReturnDraftModel>()
               .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            List<MonthlyReturnDraftModel> model = AutoMapper.Mapper.Map<List<MonthlyReturnDraftModel>>(dal.GetAll());

            return model;
        }

        public IList<MonthlyReturnDraftModel> GetByCompIdMonthEcr(string month, int Compid, string ecr)
        {
            MonthlyReturnDraftDAL dal = new MonthlyReturnDraftDAL();
            AutoMapper.Mapper.CreateMap<MonthlyReturnDraft, MonthlyReturnDraftModel>();
            AutoMapper.Mapper.CreateMap<MonthlyReturnDraft, MonthlyReturnDraftModel>()
               .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id));
            List<MonthlyReturnDraftModel> model = AutoMapper.Mapper.Map<List<MonthlyReturnDraftModel>>(dal.GetByCompIdMonthEcr(month, Compid,ecr));

            model = model.OrderBy(x => x.Name).ToList();
            return model;
        }

        public override MonthlyReturnDraftModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Insert(MonthlyReturnDraftModel obj)
        {
            throw new NotImplementedException();
        }
    }
}