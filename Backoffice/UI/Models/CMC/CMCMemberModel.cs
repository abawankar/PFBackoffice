using DAL.Master;
using Domain.Implementation;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.CMC
{
    public class CMCMemberModel : Domain.Implementation.CMCMember
    {
    }

    public class CMCMemberRepository : RepositoryModel<CMCMemberModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(CMCMemberModel obj)
        {
            CMCMemberDAL dal = new CMCMemberDAL();
            ICMCMember bl = dal.GetById(obj.Id);
            bl.Code = obj.Code;
            bl.Name = obj.Name;
            bl.Gender = obj.Gender;
            bl.FatherName = obj.FatherName;
            bl.DutyStation = obj.DutyStation;
            bl.District = obj.District;
            bl.SubRegion = obj.SubRegion;
            bl.DOB = obj.DOB;
            bl.DOC = obj.DOC;
            bl.BankAc = obj.BankAc;
            bl.IFSC = obj.IFSC;
            bl.BankName = obj.BankName;
            bl.PAN = obj.PAN;
            bl.Aadhaar = obj.Aadhaar;
            bl.Mobile = obj.Mobile;
            bl.DOE = obj.DOE;
            dal.InsertOrUpdate(bl);
        }

        public override IList<CMCMemberModel> GetAll()
        {
            CMCMemberDAL dal = new CMCMemberDAL();
            AutoMapper.Mapper.CreateMap<CMCMember, CMCMemberModel>();
            List<CMCMemberModel> model = AutoMapper.Mapper.Map<List<CMCMemberModel>>(dal.GetAll());

            return model;
        }

        public override CMCMemberModel GetById(int id)
        {
            CMCMemberDAL dal = new CMCMemberDAL();
            AutoMapper.Mapper.CreateMap<CMCMember, CMCMemberModel>();
            CMCMemberModel model = AutoMapper.Mapper.Map<CMCMemberModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(CMCMemberModel obj)
        {
            CMCMemberDAL dal = new CMCMemberDAL();
            ICMCMember bl = new CMCMember();
            bl.Code = obj.Code;
            bl.Name = obj.Name;
            bl.Gender = obj.Gender;
            bl.FatherName = obj.FatherName;
            bl.DutyStation = obj.DutyStation;
            bl.District = obj.District;
            bl.SubRegion = obj.SubRegion;
            bl.DOB = obj.DOB;
            bl.DOC = obj.DOC;
            bl.BankAc = obj.BankAc;
            bl.IFSC = obj.IFSC;
            bl.BankName = obj.BankName;
            bl.PAN = obj.PAN;
            bl.Aadhaar = obj.Aadhaar;
            bl.Mobile = obj.Mobile;
            bl.DOE = obj.DOE;
            dal.InsertOrUpdate(bl);
        }
    }

}