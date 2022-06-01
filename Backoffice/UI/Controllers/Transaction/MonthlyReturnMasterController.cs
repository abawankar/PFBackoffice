using CrystalDecisions.CrystalReports.Engine;
using DAL.Master;
using DAL.Transaction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using UI.Models.Master;
using UI.Models.Report;
using UI.Models.Transaction;

namespace UI.Controllers.Transaction
{
    [HandleError]
    [Authorize]
    public class MonthlyReturnMasterController : Controller
    {
        static List<MonthlyReturnMasterModel> statementlist = new List<MonthlyReturnMasterModel>();
        static List<Form3AModel> form3AList = new List<Form3AModel>();
        static List<Form3AModel> exportList = new List<Form3AModel>();
        static List<PMRYModel> pmry = new List<PMRYModel>();

        // GET: MonthlyReturnMaster
        //R017
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R017"))
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

            MonthlyReturnMasterDAL m = new MonthlyReturnMasterDAL();
            var month = from d in m.GetAll().OrderByDescending(x => x.Id)
                        select new { month = d.MonthYear };
            ViewBag.monthyear = new SelectList(month.Distinct(), "month", "month");

           

            return View();
        }


        [HttpPost]
        public JsonResult List(int id = 0,string monthyear=null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<MonthlyReturnMasterModel> model = new List<MonthlyReturnMasterModel>();
                MonthlyReturnMasterRepository dal = new MonthlyReturnMasterRepository();
                if(User.IsInRole("User"))
                {
                    var empRights = AppsUserDAL.GetByAppLogin(User.Identity.Name).AssignCompany;
                    var compId = from c in empRights
                                 select c.Id;
                    model = dal.GetAll().Where(c => compId.Contains(c.Id)).ToList();
                }
                else
                {
                    model = dal.GetAll().ToList();
                }
                if(!string.IsNullOrEmpty(monthyear))
                {
                    model = model.Where(x => x.MonthYear == monthyear).ToList();
                }
                if (id != 0)
                {
                    model = model.Where(x => x.CompId == id).ToList();
                }
                int count = model.Count;
                model = model.OrderByDescending(x => x.Id).ToList();
                List<MonthlyReturnMasterModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(MonthlyReturnMasterModel model)
        {
            try
            {
                MonthlyReturnMasterRepository dal = new MonthlyReturnMasterRepository();
                dal.Edit(model);
                MonthlyReturnMasterModel model1 = dal.GetById(model.Id);
                return Json(new { Result = "OK", Record = model1 });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public JsonResult FullDetails(int id=0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            List<MonthlyReturnModel> model = new List<MonthlyReturnModel>();
            MonthlyReturnMasterRepository dal = new MonthlyReturnMasterRepository();
            model = dal.GetFullDetails(id).ToList();
            int count = model.Count;
            List<MonthlyReturnModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
            return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
        }

        public ActionResult DownloadECR(int id=0)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);

            MonthlyReturnMasterDAL dal = new MonthlyReturnMasterDAL();
            var data = dal.GetById(id);
            foreach (var item in data.MonthlyReturn.Where(x=>x.UAN != ""))
            {
                if(data.ContType == "E" || data.ContType == "S") { 
                tw.WriteLine(item.UAN + "#~#" +
                             item.Name + "#~#" +
                             item.GrossWages + "#~#" +
                             item.EPFWages + "#~#" +
                             item.EPSWages + "#~#" +
                             item.EDLIWages + "#~#" +
                             item.EECont + "#~#" +
                             item.EPSCont + "#~#" +
                             item.ERCont + "#~#" +
                             item.NCPDays + "#~#0");
                }
                else
                {
                    tw.WriteLine(item.UAN + "#~#" +
                             item.Name + "#~#" +
                             item.EPFWages + "#~#" +
                             item.EPSWages + "#~#" +
                             item.EDLIWages + "#~#" +
                             item.EECont + "#~#" +
                             item.EPSCont + "#~#" +
                             item.ERCont + "#~#");
                }
            }
            tw.Flush();
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = ms;
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "plain/txt", data.Company.Name + "-PFReturn-" + data.MonthYear + ".txt");
        }

        public ActionResult ECRxls(int id =0)
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
            MonthlyReturnMasterRepository dal = new MonthlyReturnMasterRepository();
            var data = dal.GetById(id);
            int i = 1;
            foreach (var item in data.MonthlyReturn.Where(x=>x.UAN != "").ToList())
            {
                DataRow dr = dt.NewRow();
                dr["SrNo"] = i;
                dr["Branch"] = EmployeeDAL.GetBranch(data.Company.Id, item.EmpCode);
                dr["EmpCode"] = item.EmpCode;
                dr["UAN"] = "'"+item.UAN;
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
            var data1 = data.MonthlyReturn.Where(x => x.UAN != "").ToList();
            DataRow dr1 = dt.NewRow();
            dr1["Gross_Wages"] = data1.Sum(x=>x.GrossWages);
            dr1["EPF_Wages"] = data1.Sum(x => x.EPFWages);
            dr1["EPS_Wages"] = data1.Sum(x => x.EPSWages);
            dr1["EDLI_Wages"] = data1.Sum(x => x.EDLIWages);
            dr1["EPF_Contri_Remitted"] = data1.Sum(x => x.EECont);
            dr1["EPS_Contri_Remitted"] = data1.Sum(x => x.EPSCont);
            dr1["EPF_EPS_Diff_Remitted"] = data1.Sum(x => x.ERCont);
            dt.Rows.Add(dr1);

            DataRow blank = dt.NewRow();
            dt.Rows.Add(blank);

            DataRow desc = dt.NewRow();
            desc["MemberName"] = "PARTICULARS";
            desc["Gross_Wages"] = "A/c 01";
            desc["EPF_Wages"] = "A/C 02";
            desc["EPS_Wages"] = "A/C 10";
            desc["EDLI_Wages"] = "A/c 21";
            desc["EPF_Contri_Remitted"] = "Total";
            dt.Rows.Add(desc);

            DataRow ch = dt.NewRow();
            double ac2 = Math.Round((data1.Sum(x => x.EPFWages) * data.AdminAct2) / 100);
            ch["MemberName"] = "Administration Charges";
            ch["EPF_Wages"] = ac2;
            ch["EPF_Contri_Remitted"] = ac2;
            dt.Rows.Add(ch);

            DataRow ch1 = dt.NewRow();
            double er = data1.Sum(x => x.ERCont);
            double eps = data1.Sum(x => x.EPSCont);
            double edli = Math.Round((data1.Sum(x => x.EDLIWages) * data.EDLIAct21) / 100);
            ch1["MemberName"] = "Employer's Share";
            ch1["Gross_Wages"] = er;
            ch1["EPS_Wages"] = eps;
            ch1["EDLI_Wages"] = edli;
            ch1["EPF_Contri_Remitted"] = er + eps + edli;
            dt.Rows.Add(ch1);
            DataRow ch2 = dt.NewRow();
            double ee = data1.Sum(x => x.EECont);
            ch2["MemberName"] = "Employee's Share";
            ch2["Gross_Wages"] = ee;
            ch2["EPF_Contri_Remitted"] = ee;
            dt.Rows.Add(ch2);

            DataRow ch3 = dt.NewRow();
            ch3["MemberName"] = "Total";
            ch3["Gross_Wages"] = ee+er;
            ch3["EPF_Wages"] = ac2;
            ch3["EPS_Wages"] = eps;
            ch3["EDLI_Wages"] = edli;
            ch3["EPF_Contri_Remitted"] = ee+er+ac2+eps+edli;
            dt.Rows.Add(ch3);

            gv.DataSource = dt;
            gv.DataBind();

            #region -- Report Filter --

            GridViewRow row1 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            GridViewRow row2 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            GridViewRow row3 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell cell_1 = new TableCell();
            cell_1.Text = data.Company.Name;
            cell_1.BackColor = Color.Yellow;
            cell_1.Font.Bold = true;
            cell_1.ColumnSpan = 14;
            row1.Cells.Add(cell_1);

            TableCell cell_2 = new TableCell();
            cell_2.Text = "PF Code: " + data.Company.EstablishmentCode;
            cell_2.BackColor = Color.Yellow;
            cell_2.ColumnSpan = 14;
            row2.Cells.Add(cell_2);

            TableCell cell_3 = new TableCell();
            cell_3.Text = "EPF Contribution Statement For The Month of : " + data.MonthYear;
            cell_3.BackColor = Color.Yellow;
            cell_3.ColumnSpan = 14;
            row3.Cells.Add(cell_3);

            gv.Controls[0].Controls.AddAt(0, row1);
            gv.Controls[0].Controls.AddAt(1, row2);
            gv.Controls[0].Controls.AddAt(2, row3);

            #endregion

            Response.ClearContent();
            Response.Buffer = true;
            string fileName =  data.Company.Code + "-ECR-" + data.MonthYear + ".xls";
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

        public ActionResult DashboardView()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult DashboardList()
        {
            try
            {
                List<MonthlyReturnMasterModel> model = new List<MonthlyReturnMasterModel>();
                MonthlyReturnMasterRepository dal = new MonthlyReturnMasterRepository();
                //model = dal.GetAll().Where(x => x.PaymentDate == null).ToList();
                model = dal.NotPaid().ToList();
                if (User.IsInRole("User"))
                {
                    var empRights = AppsUserDAL.GetByAppLogin(User.Identity.Name).AssignCompany;
                    var compId = from c in empRights
                                 select c.Id;
                    model = model.Where(c => compId.Contains(c.Id)).ToList();
                }
                else
                {
                    model = model.OrderByDescending(x => x.Id).Take(20).ToList();
                }
               
                int count = model.Count;
                return Json(new { Result = "OK", Records =model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult PFChallan()
        {
            return View();
        }

        public ActionResult PaymentReceipt()
        {
            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile()
        {
            string messages = "";
            string _imgname = string.Empty;
            string imagepath = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                {
                    System.Web.HttpPostedFile hpf = hfc[iCnt];
                    if (hpf.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(hpf.FileName);
                        var _ext = Path.GetExtension(hpf.FileName);
                        if (_ext.ToLower() ==".pdf")
                        {
                            var _comPath = Server.MapPath(Url.Content("~/Upload/PFChallan/")) + fileName;
                            var path = _comPath;
                            hpf.SaveAs(path);
                            messages = messages==""?hpf.FileName:messages + "#~#" + hpf.FileName;
                        }
                    }
                }
            }
            return Json(Convert.ToString(messages), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadPmtRecpt()
        {
            string messages = "";
            string _imgname = string.Empty;
            string imagepath = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                {
                    System.Web.HttpPostedFile hpf = hfc[iCnt];
                    if (hpf.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(hpf.FileName);
                        var _ext = Path.GetExtension(hpf.FileName);
                        if (_ext.ToLower() == ".pdf")
                        {
                            var _comPath = Server.MapPath(Url.Content("~/Upload/PFPaymentReceipt/")) + fileName;
                            var path = _comPath;
                            hpf.SaveAs(path);
                            messages = messages == "" ? hpf.FileName : messages + "#~#" + hpf.FileName;
                        }
                    }
                }
            }
            return Json(Convert.ToString(messages), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportLastMonth()
        {
            CompanyRepository dal = new CompanyRepository();
            ViewBag.comp = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");
            ViewBag.monthyear = new SelectList(new[] {
                new SelectListItem { Text = "", Value = "" },
            }, "Value", "Text");

            return View();
        }

        public ActionResult SalaryLastMonthxls(int my = 0)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Branch", typeof(string)));
            dt.Columns.Add(new DataColumn("EmpCode", typeof(string)));
            dt.Columns.Add(new DataColumn("UAN", typeof(string)));
            dt.Columns.Add(new DataColumn("MemberName", typeof(string)));
            dt.Columns.Add(new DataColumn("Gross_Wages", typeof(string)));
            dt.Columns.Add(new DataColumn("EPF_Wages", typeof(string)));
            dt.Columns.Add(new DataColumn("NCP_Days", typeof(string)));
            dt.Columns.Add(new DataColumn("ESIWages", typeof(string)));

            GridView gv = new GridView();
            MonthlyReturnMasterRepository dal = new MonthlyReturnMasterRepository();
            var data = dal.GetById(my);
            int i = 1;
            foreach (var item in data.MonthlyReturn.OrderBy(x=>x.UAN).ThenBy(x=>x.Name).ToList())
            {
                DataRow dr = dt.NewRow();
                dr["SrNo"] = i;
                dr["Branch"] = EmployeeDAL.GetBranch(data.Company.Id, item.EmpCode);
                dr["EmpCode"] = item.EmpCode;
                dr["UAN"] = "'" + item.UAN;
                dr["MemberName"] = item.Name;
                dr["Gross_Wages"] = item.GrossWages;
                dr["EPF_Wages"] = item.EPFWages;
                dr["NCP_Days"] = item.NCPDays;
                dr["ESIWages"] = item.ESIWages;
                dt.Rows.Add(dr);
                i++;
            }
            gv.DataSource = dt;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            string fileName = data.Company.Code + "-Salary-" + data.MonthYear + ".xls";
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

        [HttpPost]
        public JsonResult GetMonthYear(int Compid)
        {
            MonthlyReturnMasterDAL dal = new MonthlyReturnMasterDAL();
            var model = dal.GetByCompId(Compid).OrderByDescending(x=>x.Id).ToList();
            model = model.Take(10).ToList();
            return Json(new { Result = model});
        }

        #region --- Repoart----

        public ActionResult Form3A()
        {
            CompanyRepository dal = new CompanyRepository();
            ViewBag.comp = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            ViewBag.SearchFields = new SelectList(new[] {
                new SelectListItem { Text = "UAN", Value = "1" },
                new SelectListItem { Text = "Name", Value = "2" },
                new SelectListItem { Text = "MemberId", Value = "3" },
            }, "Value", "Text");

            ViewBag.Status = new SelectList(new[] {
                new SelectListItem { Text = "Active", Value = "1" },
                new SelectListItem { Text = "Exited", Value = "2" },
            }, "Value", "Text");


            var now = DateTimeOffset.Now;
            var data = Enumerable.Range(0, Convert.ToInt32(ConfigurationManager.AppSettings["pf"]))
                .Select(i => new SelectListItem { Value = now.AddYears(-i).ToString("yyyy") + "-" + now.AddYears(-i+1).ToString("yyyy"), Text = now.AddYears(-i).ToString("yyyy")+"-"+ now.AddYears(-i+1).ToString("yyyy") });
            ViewBag.fy = new SelectList(data, "Value", "Text");

            return View();
        }

        [HttpPost]
        public JsonResult Form3AList(string fy=null, int compid = 0, string list=null)
        {
            var years = fy.Split('-');
            //string[] p1 = { "04-" + years[0], "05-" + years[0], "06-" + years[0], "07-" + years[0], "08-" + years[0], "09-" + years[0], "10-" + years[0], "11-" + years[0], "12-" + years[0], "01-" + years[1], "02-" + years[1], "03-" + years[1] };

            string[] p1 = { "04/" + years[0], "05/" + years[0], "06/" + years[0], "07/" + years[0], "08/" + years[0], "09/" + years[0], "10/" + years[0], "11/" + years[0], "12/" + years[0], "01/" + years[1], "02/" + years[1], "03/" + years[1] };
            try
            {
                List<Form3AModel> model = new List<Form3AModel>();
                MonthlyReturnMasterRepository dal = new MonthlyReturnMasterRepository();
                var data = from m in dal.GetByCompId(compid).ToList()
                           from r in m.MonthlyReturn.Where(x=>x.UAN != "")
                           select new Form3AModel
                           {
                               UAN = r.UAN,
                               Name = r.Name,
                               Month = r.MonthYear,
                               Wages = r.EPFWages,
                               SelfCont = r.EECont,
                               ERCont = r.ERCont,
                               EPSCont = r.EPSCont,
                               Total = r.EECont + r.ERCont+r.EPSCont
                           };
                model = data.ToList();
                model = model.Where(x => p1.Contains(x.Month)).ToList();
                List<Form3AModel> model1 = new List<Form3AModel>();
                foreach (var item in model.GroupBy(x=>x.UAN))
                {
                    Form3AModel bl = new Form3AModel();
                    bl.UAN = item.Key;
                    bl.Name = item.Select(x => x.Name).Take(1).SingleOrDefault();
                    bl.Wages = item.Sum(x => x.Wages);
                    bl.SelfCont = item.Sum(x => x.SelfCont);
                    bl.ERCont = item.Sum(x => x.ERCont);
                    bl.EPSCont = item.Sum(x => x.EPSCont);
                    bl.Total = item.Sum(x => x.Total);
                    bl.ContCount = item.Count();
                    model1.Add(bl);
                }

                form3AList = model.ToList();
                exportList = model1.ToList();
                int count = model1.Count;
                if(count == 0)
                {
                    return Json(new { Result = "Error", Message = "No contribution found for period " + fy });
                }
                return Json(new { Result = "OK", Records = model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
      
        public ActionResult Print(string list)
        {
            var uan = list.Split(',');
            var data = form3AList.Where(x => uan.Contains(x.UAN)).ToList();
            Reportdataset rds = new Reportdataset();
            int id = 1;
            foreach (var item in data.GroupBy(x=>x.UAN))
            {
                DataRow dr = rds.Tables["Form3A"].NewRow();
                dr["Id"] = id;
                dr["UAN"] = item.Key;
                dr["Name"] = item.Select(x => x.Name).Take(1).SingleOrDefault(); ;
                rds.Tables["Form3A"].Rows.Add(dr);

                foreach (var trn in item)
                {
                    DataRow drt = rds.Tables["Form3ATrn"].NewRow();
                    drt["Id"] = id;
                    drt["Month"] = trn.Month;
                    drt["Wages"] = trn.Wages;
                    drt["SelfCont"] = trn.SelfCont;
                    drt["ERCont"] = trn.ERCont;
                    drt["EPSCont"] = trn.EPSCont;
                    drt["Total"] = trn.Total;
                    rds.Tables["Form3ATrn"].Rows.Add(drt);
                }
                id++;
                
            }
            ReportDocument rd = new ReportDocument();
            string repoertname = "\\Reports\\Form3A.rpt";
            var reportpath = Server.MapPath(repoertname);
            rd.Load(reportpath);
            rd.SetDataSource(rds);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                return File(stream, "application/pdf", "Form3A" +  ".pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult Export()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("UAN", typeof(string)));
            dt.Columns.Add(new DataColumn("MemberName", typeof(string)));
            dt.Columns.Add(new DataColumn("ContCount", typeof(string)));
            dt.Columns.Add(new DataColumn("Wages", typeof(string)));
            dt.Columns.Add(new DataColumn("EmployeeCont", typeof(string)));
            dt.Columns.Add(new DataColumn("EmployerCont", typeof(string)));
            dt.Columns.Add(new DataColumn("EPSCont", typeof(string)));
            dt.Columns.Add(new DataColumn("Total", typeof(string)));

            GridView gv = new GridView();
            MonthlyReturnMasterRepository dal = new MonthlyReturnMasterRepository();
            var data = exportList.ToList();
            foreach (var item in data.OrderBy(x=>x.Name).ToList())
            {
                DataRow dr = dt.NewRow();
                dr["UAN"] = "'" + item.UAN;
                dr["MemberName"] = item.Name;
                dr["ContCount"] = item.ContCount;
                dr["Wages"] = item.Wages;
                dr["EmployeeCont"] = item.SelfCont;
                dr["EmployerCont"] = item.ERCont;
                dr["EPSCont"] = item.EPSCont;
                dr["Total"] = item.Total;
                dt.Rows.Add(dr);
            }
            DataRow dr1 = dt.NewRow();
            dr1["Wages"] = data.Sum(x => x.Wages);
            dr1["MemberName"] = data.Count();
            dr1["EmployeeCont"] = data.Sum(x => x.SelfCont);
            dr1["EmployerCont"] = data.Sum(x => x.ERCont);
            dr1["EPSCont"] = data.Sum(x => x.EPSCont);
            dr1["Total"] = data.Sum(x => x.Total);
            dt.Rows.Add(dr1);

            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "YearlyContributionFile.xls";
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
        #endregion

        public ActionResult ViewMonthlyReturn()
        {
            CompanyRepository dal = new CompanyRepository();
            ViewBag.comp = new SelectList(dal.GetAll(), "Id", "Name");

            var now = DateTimeOffset.Now;
            var data = Enumerable.Range(1,2)
                .Select(i => new SelectListItem { Value = now.AddMonths(-i).ToString("MM/yyyy"), Text = now.AddMonths(-i).ToString("MM/yyyy") });
            ViewBag.monthyear = new SelectList(data, "Value", "Text");

            return View();
        }

        public ActionResult DeleteMonthlyReturn(string month, int compid, string ecr)
        {
            MonthlyReturnMasterRepository dal = new MonthlyReturnMasterRepository();
            MonthlyReturnMasterModel bl = dal.GetByCompId(compid).Where(x => x.MonthYear == month && x.ContType == ecr).SingleOrDefault();
            string message = null;
            if (dal.Delete(bl.Id))
            {
                message = "Monthly Return Delete Successfully!";
            }
            else
            {
                message = "Issue in delete, Please contact system administrator";
            }
            
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        #region ---- PMRPY ----

        public ActionResult PMRPY()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R015"))
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

            var now = DateTimeOffset.Now;
            var data = Enumerable.Range(1, 2)
                .Select(i => new SelectListItem { Value = now.AddMonths(-i).ToString("MM/yyyy"), Text = now.AddMonths(-i).ToString("MM/yyyy") });
            ViewBag.monthyear = new SelectList(data, "Value", "Text");


            return View();
        }

        [HttpPost]
        public JsonResult PMRPYList(int compid=0,string month=null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            pmry.Clear();
            try
            {
                MonthlyReturnRepository dal = new MonthlyReturnRepository();
                List<MonthlyReturnModel> model = new List<MonthlyReturnModel>();
                model = dal.GetByCompIdMonth(month, compid).ToList();
                model = model.Where(x => x.GrossWages <= 15000).ToList();
                
                EmployeeRepository e = new EmployeeRepository();
                foreach (var item in model)
                {
                    PMRYModel bl = new PMRYModel();
                    var data = e.GetByCompAndCode(compid, item.EmpCode);
                    bl.Id = data.Id;
                    bl.CompName = data.Company.Name;
                    bl.EmpName = data.Name;
                    bl.UAN = data.UAN;
                    bl.Aaadhaar = data.Aadhaar;
                    bl.GrossWages = item.GrossWages;
                    bl.DOJ = data.DOJ;
                    pmry.Add(bl);
                }

                int count = pmry.Count;
                return Json(new { Result = "OK", Records = pmry, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdatePMRY(PMRYModel model)
        {
            try
            {
                var data = pmry.Where(x => x.Id == model.Id).SingleOrDefault();
                if (model.JobDescription != "") data.JobDescription = model.JobDescription;

                return Json(new { Result = "OK", Record = data });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult GenrateTxtFile(string list)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            string[] emp = list.Split(',');
            List<PMRYModel> model = new List<PMRYModel>();

            for (int i = 1; i < emp.Length; i++)
            {
                int id = Convert.ToInt32(emp[i]);
                var data = pmry.Where(x => x.Id == id).SingleOrDefault();
                tw.WriteLine(data.UAN + "#~##~#" +
                             data.Aaadhaar + "#~#" +
                             data.GrossWages + "#~#" +
                             data.JobDescription);
            }
            
            tw.Flush();
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = ms;
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "plain/txt", "PMRY.txt");
        }

        #endregion

    }
}