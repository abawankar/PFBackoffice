using DAL.Transaction;
using Domain.Implementation.Transaction;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Transaction
{
    public class PayslipDataModel:Domain.Implementation.Transaction.PayslipData
    {
    }

    public class PayslipMasterModel : Domain.Implementation.Transaction.PayslipMaster
    {
    }

    public enum PayData
    {
        SalMonth,
        EmpCode,
        CardNo,
        GroupCode,
        PFNo,
        UAN,
        ESI,
        Name,
        FatherName,
        WorkDay,
        Holiday,
        WeekOff,
        EBasic,
        EHra,
        EConv,
        EOther,
        ESpal,
        EExtra,
        TotalRate,
        Basic,
        Hra,
        Conv,
        Other,
        Extra9,
        Extra12,
        Other6,
        GrossPay,
        PfWorker,
        EsiWorker,
        TDS,
        Advance,
        TotalDedn,
        NetPay,
        Designation,
        AppointDt,
        PAN,
        Dept,
        EmilId,
        BankName,
        BankAccount,
        DOB
    }

    public class PayslipDataRepository : RepositoryModel<PayslipDataModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(PayslipDataModel obj)
        {
            throw new NotImplementedException();
        }

        public override IList<PayslipDataModel> GetAll()
        {
            PayslipDataDAL dal = new PayslipDataDAL();
            AutoMapper.Mapper.CreateMap<PayslipData, PayslipDataModel>();
            List<PayslipDataModel> model = AutoMapper.Mapper.Map<List<PayslipDataModel>>(dal.GetAll());

            return model;
        }

        public override PayslipDataModel GetById(int id)
        {
            PayslipDataDAL dal = new PayslipDataDAL();
            AutoMapper.Mapper.CreateMap<PayslipData, PayslipDataModel>();
            PayslipDataModel model = AutoMapper.Mapper.Map<PayslipDataModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(PayslipDataModel obj)
        {
            throw new NotImplementedException();
        }

        public void InsertBulk(List<PayslipDataModel> obj)
        {
            PayslipDataDAL dal = new PayslipDataDAL();
            IList<IPayslipData> list = new List<IPayslipData>();
            foreach (var item in obj)
            {
                IPayslipData model = new PayslipData();
                model.SalMonth=item.SalMonth;
                model.EmpCode =item.EmpCode;
                model.CardNo  =item.CardNo;
                model.GroupCode    =item.GroupCode;
                model.PFNo    =item.PFNo;
                model.UAN     =item.UAN;
                model.ESI    =item.ESI;
                model.Name    =item.Name;
                model.FatherName=item.FatherName;
                model.WorkDay =item.WorkDay;
                model.Holiday =item.Holiday;
                model.WeekOff =item.WeekOff;
                model.EBasic  =item.EBasic;
                model.EHra    =item.EHra;
                model.EConv   =item.EConv;
                model.EOther  =item.EOther;
                model.ESpal   =item.ESpal;
                model.EExtra  =item.EExtra;
                model.TotalRate=item.TotalRate;
                model.Basic   =item.Basic;
                model.Hra     =item.Hra;
                model.Conv    =item.Conv;
                model.Other   =item.Other;
                model.Extra9  =item.Extra9;
                model.Extra12 =item.Extra12;
                model.Other6  =item.Other6;
                model.GrossPay=item.GrossPay;
                model.PfWorker=item.PfWorker;
                model.EsiWorker     =item.EsiWorker;
                model.TDS     =item.TDS;
                model.Advance = item.Advance;
                model.TotalDedn   =item.TotalDedn;
                model.NetPay  =item.NetPay;
                model.Designation  = item.Designation;
                model.AppointDt    =item.AppointDt;
                model.PAN     =item.PAN;
                model.Dept = item.Dept;
                model.DOB = item.DOB;
                model.Gender = item.Gender;
                list.Add(model);
            }
            dal.InsertBulk(list);
        }
    }

    public class PayslipMasterRepository : RepositoryModel<PayslipMasterModel>
    {
        public override bool Delete(int id)
        {
            PayslipMasterDAL dal = new PayslipMasterDAL();
            IPayslipMaster emp = dal.GetById(id);
            return dal.Delete(emp);
        }

        public override void Edit(PayslipMasterModel obj)
        {
            throw new NotImplementedException();
        }

        public override IList<PayslipMasterModel> GetAll()
        {
            PayslipMasterDAL dal = new PayslipMasterDAL();
            AutoMapper.Mapper.CreateMap<PayslipMaster, PayslipMasterModel>();
            List<PayslipMasterModel> model = AutoMapper.Mapper.Map<List<PayslipMasterModel>>(dal.GetAll());

            return model;
        }


        public override PayslipMasterModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(PayslipMasterModel obj, List<PayslipDataModel> datalist )
        {
            PayslipMasterDAL dal = new PayslipMasterDAL();
            IPayslipMaster bl = new PayslipMaster();
            bl.SalMonth = obj.SalMonth;
            bl.NoofEmp = obj.NoofEmp;
            bl.Basic = obj.Basic;
            bl.Hra = obj.Hra;
            bl.Conv = obj.Conv;
            bl.Other = obj.Other;
            bl.Extra9 = obj.Extra9;
            bl.Extra12 = obj.Extra12;
            bl.Other6 = obj.Other6;
            bl.GrossPay = obj.GrossPay;
            bl.PfWorker = obj.PfWorker;
            bl.EsiWorker = obj.EsiWorker;
            bl.TDS = obj.TDS;
            bl.Advance = obj.Advance;
            bl.TotalDedn = obj.TotalDedn;
            bl.NetPay = obj.NetPay;

            IList<IPayslipData> list = new List<IPayslipData>();
            foreach (var item in datalist)
            {
                IPayslipData model = new PayslipData();
                model.SalMonth = item.SalMonth;
                model.EmpCode = item.EmpCode;
                model.CardNo = item.CardNo;
                model.GroupCode = item.GroupCode;
                model.PFNo = item.PFNo;
                model.UAN = item.UAN;
                model.ESI = item.ESI;
                model.Name = item.Name;
                model.FatherName = item.FatherName;
                model.WorkDay = item.WorkDay;
                model.Holiday = item.Holiday;
                model.WeekOff = item.WeekOff;
                model.Basic = item.Basic;
                model.Hra = item.Hra;
                model.Conv = item.Conv;
                model.Other = item.Other;
                model.Extra9 = item.Extra9;
                model.Extra12 = item.Extra12;
                model.Other6 = item.Other6;
                model.GrossPay = item.GrossPay;
                model.PfWorker = item.PfWorker;
                model.EsiWorker = item.EsiWorker;
                model.TDS = item.TDS;
                model.Advance = item.Advance;
                model.TotalDedn = item.TotalDedn;
                model.NetPay = item.NetPay;
                model.Designation = item.Designation;
                model.AppointDt = item.AppointDt;
                model.PAN = item.PAN;
                model.Dept = item.Dept;
                model.Emailid = item.Emailid;
                model.BankName = item.BankName;
                model.BankAccount = item.BankAccount;
                model.DOB = item.DOB;
                model.MailSent = MailSent.No;
                list.Add(model);
            }
            bl.PayslipData = list.ToList();
            dal.InsertOrUpdate(bl);
        }

        public override void Insert(PayslipMasterModel obj)
        {
            throw new NotImplementedException();
        }

        public IList<PayslipDataModel> GetFullDetails(int id)
        {
            PayslipMasterDAL dal = new PayslipMasterDAL();
            AutoMapper.Mapper.CreateMap<PayslipData, PayslipDataModel>();
            List<PayslipDataModel> model = AutoMapper.Mapper.Map<List<PayslipDataModel>>(dal.GetById(id).PayslipData.ToList());

            return model;
        }
    }
}