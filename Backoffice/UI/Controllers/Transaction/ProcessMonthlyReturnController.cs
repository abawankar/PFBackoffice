using DAL.Master;
using DAL.Transaction;
using Domain.Implementation;
using Domain.Implementation.Transaction;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using UI.Models.Master;
using UI.Models.Transaction;
using System.Net.Mail;

namespace UI.Controllers.Transaction
{
    [HandleError]
    [Authorize]
    public class ProcessMonthlyReturnController : Controller
    {
        // GET: ProcessMonthlyReturn
        //R016
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R016"))
            {
            }
            else {
                return View("Security");
            }
            if (User.IsInRole("User"))
            {
                var comp = AppsUserDAL.GetByAppLogin(User.Identity.Name).AssignCompany.ToList();
                ViewBag.comp = new SelectList(comp, "Id", "Name");
            }
            else
            {
                CompanyRepository dal = new CompanyRepository();
                ViewBag.comp = new SelectList(dal.GetAll(), "Id", "Name");
            }

            return View();
        }

        [HttpPost]
        public JsonResult DraftList(int id =0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<DraftMonthlyReturnModel> model = new List<DraftMonthlyReturnModel>();
                DraftMonthlyReturnRepository dal = new DraftMonthlyReturnRepository();
                model = dal.GetAll().ToList();
                if(id != 0)
                {
                    model = model.Where(x => x.CompId == id).ToList();
                }
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public JsonResult DeleteDraft(string month, int compid, string ecr)
        {
            MonthlyReturnMasterDAL process = new MonthlyReturnMasterDAL();
            var data = process.DeleteDraftReturn(month, compid, ecr);
            string message = "Drafted Return Delete Successfully!";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Process(string month, int compid,string ecr)
        {
            string message = "";
            string compname = "";
            MonthlyReturnMasterDAL process = new MonthlyReturnMasterDAL();
            MonthlyReturnMasterRepository chk = new MonthlyReturnMasterRepository();

            var result = chk.GetAll().Where(x => x.MonthYear == month && x.Company.Id == compid && x.ContType == ecr);
            if(result.Count() != 0)
            {
                message = "ECR Already process for this month!";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            process.UpdateMissingUAN(month, compid, ecr);
            List<IMonthlyReturn> model = new List<IMonthlyReturn>();
            MonthlyReturnDraftRepository dal = new MonthlyReturnDraftRepository();
            var data = dal.GetByCompIdMonthEcr(month, compid, ecr).ToList();

            if(ecr =="S")
            {
                if(data.Where(x=>x.UAN =="").Count() != 0)
                {
                    return Json("UAN is not assigne, Please do same", JsonRequestBehavior.AllowGet);
                }
            }
            double Act10 = 0;
            double AdminAct2 = 0;
            double EDLIAct21 = 0;
            
            foreach (var obj in data)
            {
                IMonthlyReturn bl = new MonthlyReturn();
                bl.Branch = obj.Branch;
                bl.ContType = obj.ContType;
                bl.MonthYear = obj.MonthYear;
                bl.EmpCode = obj.EmpCode;
                bl.Name = obj.Name;
                bl.UAN = obj.UAN;
                bl.GrossWages = obj.GrossWages;
                bl.EPFWages = obj.EPFWages;
                bl.EPSWages = obj.EPSWages;
                bl.EDLIWages = obj.EDLIWages;
                bl.EECont = obj.EECont;
                bl.EPSCont = obj.EPSCont;
                bl.ERCont = obj.ERCont;
                bl.NCPDays = obj.NCPDays;
                bl.ESIWages = obj.ESIWages;
                model.Add(bl);

                Act10 = obj.Company.StatutoryCode.Act10;
                AdminAct2 = obj.Company.StatutoryCode.AdminAct2;
                EDLIAct21 = obj.Company.StatutoryCode.EDLIAct21;
                compname = obj.Company.Name;

            }
            
            IMonthlyReturnMaster trn = new MonthlyReturnMaster();
            trn.ContType = ecr;
            trn.Company = new Company { Id = compid };
            trn.MonthYear = month;
            trn.MonthlyReturn = model;
            trn.Act10 = Act10;
            trn.AdminAct2 = AdminAct2;
            trn.EDLIAct21 = EDLIAct21;
            process.InsertOrUpdate(trn);
            process.DeleteDraftReturn(month, compid, ecr);
            message = "Return Process Successfully";
            try
            {
                MailMessage msg = new MailMessage();
                msg.To.Add(new MailAddress("Savvybackoffice@gmail.com"));
                msg.Subject = "PF Contribution " + month + " Company:" + compname;
                //msg.Bcc.Add("abawankar@gmail.com");
                msg.Body = "<p>Hi</p> <p>Below PF Contribution details process</p>" +
                    "<p>Month : " + month + "</p>" +
                    "<p> Company Name : " + compname + "</p>" + 
                    "<p> Process by :" + User.Identity.Name +"</p>" +
                    "<p> Regards /Portal Admin</p>";
                msg.IsBodyHtml = true;
                Common.EmailSetting.SendEmail(msg);
            }
            catch { };

            return Json(message, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult DraftFullDetails(string month, int compid, string ecr, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            List<MonthlyReturnDraftModel> model = new List<MonthlyReturnDraftModel>();
            MonthlyReturnDraftRepository dal = new MonthlyReturnDraftRepository();
            model = dal.GetByCompIdMonthEcr(month, compid, ecr).ToList();
            int count = model.Count;
            List<MonthlyReturnDraftModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
            return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
        }

        [HttpPost]
        public JsonResult UpdateReturn(MonthlyReturnDraftModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                MonthlyReturnDraftRepository dal = new MonthlyReturnDraftRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult ECRxls(string month, int compid, string ecr)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Branch", typeof(string)));
            dt.Columns.Add(new DataColumn("EmpCode", typeof(string)));
            dt.Columns.Add(new DataColumn("UAN", typeof(string)));
            dt.Columns.Add(new DataColumn("MemberName", typeof(string)));
            dt.Columns.Add(new DataColumn("Gross_Wages", typeof(string)));
            dt.Columns.Add(new DataColumn("EPF_Wages", typeof(string)));
            dt.Columns.Add(new DataColumn("EPS_Wages", typeof(string)));
            dt.Columns.Add(new DataColumn("EDLI_Wages", typeof(string)));
            dt.Columns.Add(new DataColumn("EPF_Contri_Remitted", typeof(string)));
            dt.Columns.Add(new DataColumn("EPS_Contri_Remitted", typeof(string)));
            dt.Columns.Add(new DataColumn("EPF_EPS_Diff_Remitted", typeof(string)));
            dt.Columns.Add(new DataColumn("NCP_Days", typeof(string)));
            dt.Columns.Add(new DataColumn("Refun_Of_Adcances", typeof(string)));

            GridView gv = new GridView();
            MonthlyReturnDraftRepository dal = new MonthlyReturnDraftRepository();
            var data = dal.GetByCompIdMonthEcr(month, compid, ecr).ToList();
            int i = 1;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["SrNo"] = i;
                dr["Branch"] = item.Branch;
                dr["EmpCode"] = item.EmpCode;
                dr["UAN"] = "'" + item.UAN;
                dr["MemberName"] = item.Name;
                dr["Gross_Wages"] = item.GrossWages;
                dr["EPF_Wages"] = item.EPFWages;
                dr["EPS_Wages"] = item.EPSWages;
                dr["EDLI_Wages"] = item.EDLIWages;
                dr["EPF_Contri_Remitted"] = item.EECont;
                dr["EPS_Contri_Remitted"] = item.EPSCont;
                dr["EPF_EPS_Diff_Remitted"] = item.ERCont;
                dr["NCP_Days"] = item.NCPDays;
                dr["Refun_Of_Adcances"] = "";
                dt.Rows.Add(dr);
                i++;
            }
            gv.DataSource = dt;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "ContributionFile.xls";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);

            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult UpdateUAN(int CompId,string ECRType,string MonthYear)      //For Updating changes   
        {
            try
            {
                MonthlyReturnDraftRepository dal = new MonthlyReturnDraftRepository();
                var data = dal.GetByCompIdMonthEcr(MonthYear, CompId, ECRType).ToList();
                EmployeeRepository emp = new EmployeeRepository();

                MonthlyReturnDraftDAL d = new MonthlyReturnDraftDAL();
                foreach (var item in data)
                {
                    var bl = emp.GetByCompAndCode(CompId,item.EmpCode);
                    IMonthlyReturnDraft m = d.GetById(item.Id);
                    m.UAN = bl.UAN;
                    d.InsertOrUpdate(m);
                }
                string message = "Done";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetDraftPF()      //For Updating changes   
        {
            try
            {
                DraftMonthlyReturnRepository dal = new DraftMonthlyReturnRepository();
                var data = dal.GetAll().ToList();
                string message = data.Count().ToString();
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}