using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Transaction
{

    public class FooterModel
    {
        public int EPFEmp { get; set; }
        public int EPSEmp { get; set; }
        public int ExemptedEmp { get; set; }
        public double ExemptedGrossWages { get; set; }
        public double ExemptedWages { get; set; }
        public double EPFGrossWages { get; set; }
        public double EPFWages { get; set; }
        public double EPSWages { get; set; }
        public double EDLIWages { get; set; }
        public double EECont { get; set; }
        public double ERCont { get; set; }
        public double EPSCont { get; set; }
        public double NCPDays { get; set; }
        public double ESIWages { get; set; }
    }
    public class DraftMonthlyReturnModel
    {
        public string ECRType { get; set; }
        public string MonthYear { get; set; }
        public int CompId { get; set; }
        public int EPFEmp { get; set; }
        public int EPSEmp { get; set; }
        public int ExemptedEmp { get; set; }
        public double ExemptedGrossWages { get; set; }
        public double ExemptedWages { get; set; }
        public double EPFGrossWages { get; set; }
        public double EPFWages { get; set; }
        public double EPSWages { get; set; }
        public double EECont { get; set; }
        public double ERCont { get; set; }
        public double EPSCont { get; set; }
        public double NCPDays { get; set; }
        public double ESIWages { get; set; }
    }

    public class DraftMonthlyReturnRepository : RepositoryModel<DraftMonthlyReturnModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(DraftMonthlyReturnModel obj)
        {
            throw new NotImplementedException();
        }

        public override IList<DraftMonthlyReturnModel> GetAll()
        {
            List<DraftMonthlyReturnModel> model = new List<DraftMonthlyReturnModel>();
            MonthlyReturnDraftRepository dal = new MonthlyReturnDraftRepository();
            foreach (var comp in dal.GetAll().GroupBy(x=>x.Company.Id))
            {
                foreach (var month in comp.GroupBy(x=>x.MonthYear))
                {
                    foreach (var ecr in month.GroupBy(x=>x.ContType))
                    {
                        DraftMonthlyReturnModel bl = new DraftMonthlyReturnModel();
                        bl.CompId = comp.Key;
                        bl.MonthYear = month.Key;
                        bl.ECRType = ecr.Key;
                        bl.ExemptedEmp = ecr.Where(x => x.IsPF == "Y").Count();
                        bl.EPFEmp = ecr.Count() - bl.ExemptedEmp;
                        bl.EPSEmp = bl.EPFEmp - ecr.Where(x => x.EPSWages == 0).Count() + bl.ExemptedEmp;
                        bl.ExemptedGrossWages = ecr.Where(x => x.IsPF == "Y").Sum(x => x.GrossWages);
                        bl.ExemptedWages = ecr.Where(x => x.IsPF == "Y").Sum(x => x.EPFWages);
                        bl.EPFGrossWages = ecr.Sum(x => x.GrossWages) - bl.ExemptedGrossWages;
                        bl.EPFWages = ecr.Sum(x => x.EPFWages) - bl.ExemptedWages;
                        bl.EPSWages = ecr.Sum(x => x.EPSWages);
                        bl.EECont = ecr.Sum(x => x.EECont);
                        bl.ERCont = ecr.Sum(x => x.ERCont);
                        bl.EPSCont = ecr.Sum(x => x.EPSCont);
                        bl.NCPDays = ecr.Sum(x => x.NCPDays);
                        bl.ESIWages = ecr.Sum(x => x.ESIWages);
                        model.Add(bl);
                    }
                }
            }
            return model;
        }

        public override DraftMonthlyReturnModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Insert(DraftMonthlyReturnModel obj)
        {
            throw new NotImplementedException();
        }
    }

}