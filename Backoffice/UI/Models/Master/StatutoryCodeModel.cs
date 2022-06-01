using DAL.Master;
using Domain.Implementation;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Master
{
    public class StatutoryCodeModel : Domain.Implementation.StatutoryCode
    {

    }

    public class StatutoryCodeRepository : RepositoryModel<StatutoryCodeModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(StatutoryCodeModel obj)
        {
            StatutoryCodeDAL dal = new StatutoryCodeDAL();
            IStatutoryCode bl = dal.GetById(obj.Id);
            bl.Code = obj.Code;
            bl.PFPercent = obj.PFPercent;
            bl.AdminAct2 = obj.AdminAct2;
            bl.Act10 = obj.Act10;
            bl.EDLIAct21 = obj.EDLIAct21;
            bl.AdminAct22 = obj.AdminAct22;
            bl.AdminMinChar = obj.AdminMinChar;
            bl.EPSLimit = obj.EPSLimit;
            bl.PFLimit = obj.PFLimit;
            bl.ESIEmpRate = obj.ESIEmpRate;
            bl.ESIERRate = obj.ESIERRate;
            bl.ESILimit = obj.ESILimit;
            dal.InsertOrUpdate(bl);
        }

        public override IList<StatutoryCodeModel> GetAll()
        {
            StatutoryCodeDAL dal = new StatutoryCodeDAL();
            AutoMapper.Mapper.CreateMap<StatutoryCode, StatutoryCodeModel>();
            List<StatutoryCodeModel> model = AutoMapper.Mapper.Map<List<StatutoryCodeModel>>(dal.GetAll());

            return model;
        }

        public override StatutoryCodeModel GetById(int id)
        {
            StatutoryCodeDAL dal = new StatutoryCodeDAL();
            AutoMapper.Mapper.CreateMap<StatutoryCode, StatutoryCodeModel>();
            StatutoryCodeModel model = AutoMapper.Mapper.Map<StatutoryCodeModel>(dal.GetAll());

            return model;
        }

        public override void Insert(StatutoryCodeModel obj)
        {
            StatutoryCodeDAL dal = new StatutoryCodeDAL();
            IStatutoryCode bl = new StatutoryCode();
            bl.Code = obj.Code;
            bl.PFPercent = obj.PFPercent;
            bl.AdminAct2 = obj.AdminAct2;
            bl.Act10 = obj.Act10;
            bl.EDLIAct21 = obj.EDLIAct21;
            bl.AdminAct22 = obj.AdminAct22;
            bl.AdminMinChar = obj.AdminMinChar;
            bl.EPSLimit = obj.EPSLimit;
            bl.PFLimit = obj.PFLimit;
            bl.ESIEmpRate = obj.ESIEmpRate;
            bl.ESIERRate = obj.ESIERRate;
            bl.ESILimit = obj.ESILimit;
            dal.InsertOrUpdate(bl);
        }
    }
}