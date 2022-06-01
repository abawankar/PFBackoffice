using CrystalDecisions.CrystalReports.Engine;
using DAL.Master;
using DAL.Transaction;
using Domain.Implementation;
using Domain.Implementation.Transaction;
using Domain.Interface;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using UI.Models.Master;
using UI.Models.Transaction;

namespace UI.Controllers.Transaction
{
    public class ESIMonthlyContMasterController : Controller
    {
        enum empESI { Branch, EmpCode, EmpName, IP, ESIWages, Days, DOL };

        static List<ESIMonthlyContModel> contlist = new List<ESIMonthlyContModel>();
        static List<ESIMonthlyContMasterModel> esistatement = new List<ESIMonthlyContMasterModel>();

        // GET: ESIMonthlyContMaster

        //R019
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("E019"))
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
        public JsonResult List(int id = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<ESIMonthlyContMasterModel> model = new List<ESIMonthlyContMasterModel>();
                ESIMonthlyContMasterRepository dal = new ESIMonthlyContMasterRepository();
                if (User.IsInRole("User"))
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
                if (id != 0)
                {
                    model = model.Where(x => x.CompId == id).ToList();
                }
                int count = model.Count;
                model = model.OrderByDescending(x => x.Id).ToList();
                List<ESIMonthlyContMasterModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //R018
        public ActionResult PrepareMonthlyContribution()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R018"))
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

            ViewBag.ECRType = new SelectList(new[] {
                new SelectListItem { Text = "MonthlyContribution", Value = "M" },
                new SelectListItem { Text = "SupplimentaryContribution", Value = "S" },
            }, "Value", "Text");

            var now = DateTimeOffset.Now;
            var data = Enumerable.Range(1, 13)
                .Select(i => new SelectListItem { Value = now.AddMonths(-i).ToString("MM/yyyy"), Text = now.AddMonths(-i).ToString("MM/yyyy") });
            ViewBag.Months = new SelectList(data, "Value", "Text");
            return View();
        }

        [HttpPost]
        public JsonResult Update(ESIMonthlyContMasterModel model)
        {
            try
            {
                ESIMonthlyContMasterRepository dal = new ESIMonthlyContMasterRepository();
                dal.Edit(model);
                ESIMonthlyContMasterModel model1 = dal.GetById(model.Id);
                return Json(new { Result = "OK", Record = model1 });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public JsonResult FullDetails(int id = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            List<ESIMonthlyContModel> model = new List<ESIMonthlyContModel>();
            ESIMonthlyContMasterRepository dal = new ESIMonthlyContMasterRepository();
            model = dal.GetFullDetails(id).ToList();
            int count = model.Count;
            List<ESIMonthlyContModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
            return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
        }

        [HttpPost]
        public JsonResult DraftContList(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<ESIMonthlyContModel> model = new List<ESIMonthlyContModel>();
                model = contlist.ToList();
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateDraft(ESIMonthlyContModel model)
        {
            try
            {
                var data = contlist.Where(x => x.EmpCode == model.EmpCode).SingleOrDefault();
                data.GrossWages = model.GrossWages;
                data.IPCont = model.IPCont;
                data.Days = model.Days;
                return Json(new { Result = "OK", Record = data });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public JsonResult LastMonthReturn(int compid, string month, string ecr)
        {
            contlist.Clear();
            string message = "";
            ESIMonthlyContMasterRepository chk = new ESIMonthlyContMasterRepository();
            var result = chk.GetAll().Where(x => x.MonthYear == month && x.Company.Id == compid && x.ContType == ecr).ToList();
            if (result.Count() != 0)
            {
                message = "Error: Contributon Already process for this month!";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            MonthlyReturnMasterRepository dal = new MonthlyReturnMasterRepository();
            var data = dal.GetById(month, compid,"E");
            int days = DateTime.DaysInMonth(Convert.ToInt32(month.Substring(3, 4)), Convert.ToInt32(month.Substring(0, 2)));
            int sr = 1;
            if (data != null)
            {
                foreach (var item in data.MonthlyReturn)
                {
                    ESIMonthlyContModel bl = new ESIMonthlyContModel();
                    EmployeeRepository empdal = new EmployeeRepository();
                    EmployeeModel emp = empdal.GetByCompAndCode(data.Company.Id, item.EmpCode);
                    if(emp.ESIExempted == "N")
                    {
                        bl.Id = sr;
                        sr++;
                        bl.CompId = data.Company.Id;
                        bl.Branch = item.Branch;
                        bl.EmpCode = emp.EmpCode;
                        bl.Name = emp.Name;
                        bl.IP = emp.ESIIP;
                        bl.Days = days - item.NCPDays;
                        bl.GrossWages = item.ESIWages==0?item.GrossWages: item.ESIWages;
                        bl.IPCont = Math.Ceiling((bl.GrossWages * data.Company.StatutoryCode.ESIEmpRate) / 100);
                        bl.ContType = ecr;
                        bl.MonthYear = month;
                        bl.CompName = emp.Company.Name;
                        contlist.Add(bl);
                    }
                }
                message = "Success: Data uploaded successfully!";
            }
            else
            {
                message = "Error: No Return Found!";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FooterData()
        {
            try
            {
                List<ESIFooterModel> model = new List<ESIFooterModel>();
                var data = contlist.ToList();
                ESIFooterModel bl = new ESIFooterModel();
                bl.ESIMember = data.Count();
                bl.TotalIP = data.Sum(x => x.IPCont);
                bl.TotalER = Math.Ceiling(((data.Sum(x => x.GrossWages))*4.75)/100);
                bl.TotalCont = bl.TotalIP + bl.TotalER;
                bl.TotalGovtCont = 0;
                bl.TotalWages = data.Sum(x => x.GrossWages);
                model.Add(bl);
                return Json(new { Result = "OK", Records = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
        
        public JsonResult Process()
        {
            string message = "";
            string compname = "";
            IList<IESIMonthlyCont> list = new List<IESIMonthlyCont>();
            int compid = 0;
            string month = "";
            string type = "";
            EmployeeDAL empdal = new EmployeeDAL();
            foreach (var obj in contlist)
            {
                IESIMonthlyCont bl = new ESIMonthlyCont();
                bl.Branch = obj.Branch;
                bl.ContType = obj.ContType;
                bl.MonthYear = obj.MonthYear;
                bl.EmpCode = obj.EmpCode;
                bl.Name = obj.Name;
                bl.IP = obj.IP;
                bl.GrossWages = obj.GrossWages;
                bl.IPCont = obj.IPCont;
                bl.Days = obj.Days;
                compid = obj.CompId;
                month = obj.MonthYear;
                type = obj.ContType;
                bl.DOL = obj.DOL;
                compname = obj.CompName;
                if (obj.DOL != null)
                {
                    IEmployee emp = empdal.GetByCompAndCode(obj.CompId, obj.EmpCode);
                    emp.DOE = obj.DOL;
                    emp.Status = 2;
                    emp.IPStatus = 2;
                    empdal.InsertOrUpdate(emp);
                }
                list.Add(bl);
            }

            ESIMonthlyContMasterRepository chk = new ESIMonthlyContMasterRepository();
            var result = chk.GetAll().Where(x => x.MonthYear == month && x.Company.Id == compid && x.ContType == type);
            if (result.Count() != 0)
            {
                message = "Contributon Already process for this month!";
                contlist.Clear();
                return Json(message, JsonRequestBehavior.AllowGet);
            }


            ESIMonthlyContMasterDAL process = new ESIMonthlyContMasterDAL();
            IESIMonthlyContMaster trn = new ESIMonthlyContMaster();
            trn.Company = new Company {Id=compid };
            trn.MonthYear = month;
            trn.ContType = type;
            trn.MonthlyCont = list;
            trn.ERCont = Math.Ceiling(((list.Sum(x => x.GrossWages)) * 4.75) / 100);
            process.InsertOrUpdate(trn);
            contlist.Clear();
            message = "Saved Successfully";

            try
            {
                MailMessage msg = new MailMessage();
                msg.To.Add(new MailAddress("Savvybackoffice@gmail.com"));
                msg.Subject = "ESI Contribution " + month + " Company:" + compname;
                //msg.Bcc.Add("abawankar@gmail.com");
                msg.Body = "<p>Hi</p> <p>Below ESI Contribution details process</p>" +
                    "<p>Month : " + month + "</p>" +
                    "<p> Company Name : " + compname + "</p>" +
                    "<p> Process by :" + User.Identity.Name + "</p>" +
                    "<p> Regards /Portal Admin</p>";
                msg.IsBodyHtml = true;
                Common.EmailSetting.SendEmail(msg);
            }
            catch { };

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ECRxls(int id = 0)
        {
            ESIMonthlyContMasterRepository dal = new ESIMonthlyContMasterRepository();
            var data = dal.GetById(id);
            if(data.MonthlyCont.GroupBy(x=>x.Branch).Count() ==1)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("IP Number", typeof(string)));
                dt.Columns.Add(new DataColumn("IP Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Days", typeof(string)));
                dt.Columns.Add(new DataColumn("TotalMontlyWages", typeof(string)));
                dt.Columns.Add(new DataColumn("Code", typeof(string)));
                dt.Columns.Add(new DataColumn("LastWorkingDays", typeof(string)));

                GridView gv = new GridView();

                foreach (var item in data.MonthlyCont.ToList())
                {
                    DataRow dr = dt.NewRow();
                    dr["IP Number"] = item.IP;
                    dr["IP Name"] = item.Name;
                    dr["Days"] = item.Days;
                    dr["TotalMontlyWages"] = item.GrossWages;
                    if (item.DOL != null)
                    {
                        dr["Code"] = "2";
                        dr["LastWorkingDays"] = Convert.ToDateTime(item.DOL).ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        dr["Code"] = "";
                        dr["LastWorkingDays"] = "";
                    }

                    dt.Rows.Add(dr);
                }
                gv.DataSource = dt;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                string fileName = data.Company.Code + "ESI" + ".xls";
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);

                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Branch", typeof(string)));
                dt.Columns.Add(new DataColumn("IP Number", typeof(string)));
                dt.Columns.Add(new DataColumn("IP Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Days", typeof(string)));
                dt.Columns.Add(new DataColumn("TotalMontlyWages", typeof(string)));
                dt.Columns.Add(new DataColumn("Code", typeof(string)));
                dt.Columns.Add(new DataColumn("LastWorkingDays", typeof(string)));

                GridView gv = new GridView();

                foreach (var item in data.MonthlyCont.OrderBy(x=>x.Branch).ToList())
                {
                    DataRow dr = dt.NewRow();
                    dr["Branch"] = item.Branch;
                    dr["IP Number"] = item.IP;
                    dr["IP Name"] = item.Name;
                    dr["Days"] = item.Days;
                    dr["TotalMontlyWages"] = item.GrossWages;
                    if (item.DOL != null)
                    {
                        dr["Code"] = "2";
                        dr["LastWorkingDays"] = Convert.ToDateTime(item.DOL).ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        dr["Code"] = "";
                        dr["LastWorkingDays"] = "";
                    }

                    dt.Rows.Add(dr);
                }
                gv.DataSource = dt;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                string fileName = data.Company.Code + "ESI" + ".xls";
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);

                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

            }

            return RedirectToAction("Index");
        }

        public ActionResult ECRxls2(int id = 0)
        {


            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("IP Number", typeof(string)));
            dt.Columns.Add(new DataColumn("IP Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Days", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalMontlyWages", typeof(string)));
            dt.Columns.Add(new DataColumn("Code", typeof(string)));
            dt.Columns.Add(new DataColumn("LastWorkingDays", typeof(string)));

            GridView gv = new GridView();
            ESIMonthlyContMasterRepository dal = new ESIMonthlyContMasterRepository();
            var data = dal.GetById(id);
            foreach (var Branch in data.MonthlyCont.GroupBy(x=>x.Branch))
            {
                foreach (var item in Branch.ToList())
                {
                    DataRow dr = dt.NewRow();
                    dr["IP Number"] = item.IP;
                    dr["IP Name"] = item.Name;
                    dr["Days"] = item.Days;
                    dr["TotalMontlyWages"] = item.GrossWages;
                    if (item.DOL != null)
                    {
                        dr["Code"] = "2";
                        dr["LastWorkingDays"] = Convert.ToDateTime(item.DOL).ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        dr["Code"] = "";
                        dr["LastWorkingDays"] = "";
                    }

                    dt.Rows.Add(dr);
                }
                gv.DataSource = dt;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                string fileName = data.Company.Code + "ESI-" + Branch.Key + ".xls";
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);

                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }

            return RedirectToAction("Index");
        }

        public ActionResult PrintECR(int id)
        {
            ESIMonthlyContMasterRepository dal = new ESIMonthlyContMasterRepository();
            var data = dal.GetById(id);
            Reportdataset rds = new Reportdataset();
            DataRow dr = rds.Tables["ESIMaster"].NewRow();
            dr["Id"] = data.Id;
            dr["CompanyName"] = data.Company.ESIRegistrationNumber;
            dr["TotalIpCont"] = data.TotalIP;
            dr["ERCont"] = data.ERCont;
            dr["TotalCont"] = data.TotalIP+ data.ERCont;
            dr["TotalGrossWages"] = data.MonthlyCont.Sum(x => x.GrossWages);
            dr["MonthYear"] = data.MonthYear;
            dr["ChallanNo"] = data.ChallanNo;
            rds.Tables["ESIMaster"].Rows.Add(dr);
            foreach (var item in data.MonthlyCont)
            {
                DataRow drt = rds.Tables["ESITran"].NewRow();
                drt["Id"] = data.Id;
                drt["IP"] = item.IP;
                drt["IPName"] = item.Name;
                drt["Days"] = item.Days;
                drt["Wages"] = item.GrossWages;
                drt["IPCont"] = item.IPCont;
                rds.Tables["ESITran"].Rows.Add(drt);
            }
            ReportDocument rd = new ReportDocument();
            string repoertname = "\\Reports\\ESIECR.rpt";
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

                return File(stream, "application/pdf", "ESI-ECR-" + data.Company.Code + ".pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile(int compid, string ECRType, string months)
        {
            contlist.Clear();
            var dirPath = Server.MapPath(Url.Content("~/Upload/Temp/"));
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(dirPath);
            foreach (FileInfo fi in directory.GetFiles())
            {
                fi.IsReadOnly = false;
                fi.Delete();
            }
            CompanyRepository dal = new CompanyRepository();
            CompanyModel comp = dal.GetById(compid);

            EmployeeRepository empdal = new EmployeeRepository();

            string error = string.Empty;
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

                        if (_ext.ToLower() == ".csv")
                        {
                            var _comPath = Server.MapPath(Url.Content("~/Upload/Temp/")) + hpf.FileName;
                            var path = _comPath;
                            hpf.SaveAs(path);

                            try
                            {
                                contlist.Clear();
                                var csv = from line in System.IO.File.ReadLines(_comPath)
                                          select (line.Split(',')).ToArray();
                                int sr = 1;

                                foreach (var x in csv.Skip(1))
                                {
                                    try
                                    {

                                        ESIMonthlyContModel bl = new ESIMonthlyContModel();
                                        bl.Id = sr;
                                        sr++;
                                        bl.Branch = x[(int)empESI.Branch].Trim();
                                        bl.EmpCode = x[(int)empESI.EmpCode].Trim();
                                        bl.IP = x[(int)empESI.IP].Trim();
                                        bl.GrossWages = Convert.ToDouble(x[(int)empESI.ESIWages].Trim());
                                        bl.Days = Convert.ToInt32(x[(int)empESI.Days].Trim());
                                        EmployeeModel emp = empdal.GetByCompAndCode(comp.Id, bl.EmpCode);
                                        bl.IPCont = Math.Ceiling((bl.GrossWages * comp.StatutoryCode.ESIEmpRate) / 100);
                                        bl.ContType = ECRType;
                                        bl.MonthYear = months;
                                        bl.CompId = compid;
                                        bl.CompName = emp.Company.Name;
                                        bl.Name = emp.Name;
                                        if (!string.IsNullOrEmpty(x[(int)empESI.DOL].Trim()))
                                        {
                                            bl.DOL = Convert.ToDateTime(Convert.ToDateTime(x[(int)empESI.DOL].Trim()).ToString("yyyy-MM-dd"));
                                        }
                                        contlist.Add(bl);
                                    }
                                    catch (Exception ex) { error = error + "," + (sr - 1).ToString() + ex.Message; }
                                }

                            }
                            catch (Exception ex) { return Json(Convert.ToString(ex.Message), JsonRequestBehavior.AllowGet); }
                        }
                        else
                        {
                            var _comPath = Server.MapPath(Url.Content("~/Uploads/TempCV/")) + hpf.FileName;
                            var path = _comPath;
                            hpf.SaveAs(path);
                        }
                    }
                }
            }
            return Json(Convert.ToString(error == "" ? "No Error Found in Record" : "Error Found in Record" + error), JsonRequestBehavior.AllowGet);
        }


        #region ----- Reports -----

        public ActionResult YearlyStatement()
        {
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
            var data = Enumerable.Range(0, Convert.ToInt32(ConfigurationManager.AppSettings["fy"]))
                .Select(i => new SelectListItem { Value = now.AddYears(-i).ToString("yyyy") + "-" + now.AddYears(-i + 1).ToString("yyyy"), Text = now.AddYears(-i).ToString("yyyy") + "-" + now.AddYears(-i + 1).ToString("yyyy") });
            ViewBag.fy = new SelectList(data, "Value", "Text");
           

            return View();
        }

        [HttpPost]
        public JsonResult ESIStatementList(int compid = 0, string fy=null)
        {
            var years = fy.Split('-');

            string[] p1 = { "04/" + years[0], "05/" + years[0], "06/" + years[0], "07/" + years[0], "08/" + years[0], "09/" + years[0], "10/" + years[0], "11/" + years[0], "12/" + years[0], "01/" + years[1], "02/" + years[1], "03/" + years[1] };
            try
            {
                List<ESIMonthlyContMasterModel> model = new List<ESIMonthlyContMasterModel>();
                ESIMonthlyContMasterRepository dal = new ESIMonthlyContMasterRepository();
                model = dal.GetByCompId(compid).ToList();
                model = model.Where(x => p1.Contains(x.MonthYear)).ToList();
                int count = model.Count;
                esistatement = model.ToList();
                if (count == 0)
                {
                    return Json(new { Result = "Error", Message = "Sorry No Contribution Found!" });
                }
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult PrintStatement1(string fy)
        {
            var data = esistatement.ToList();
            string comp = null;
            string code = null;
            Reportdataset rds = new Reportdataset();
            DataRow dr = rds.Tables["ESIHalfStatement"].NewRow();
            dr["Id"] = 1;
            foreach (var item in data.OrderBy(x=>x.Id))
            {
                DataRow drt = rds.Tables["ESIHalfStatementTrn"].NewRow();
                double IPCont = item.MonthlyCont.Sum(x => x.IPCont);
                drt["Id"] = 1;
                drt["NoOfEmp"] = item.TotalIP;
                drt["GrossWages"] = item.TotalWages;
                drt["Month"] = item.MonthYear;
                drt["IPCont"] = IPCont;
                drt["ERCont"] = item.ERCont;
                drt["TotalCont"] = IPCont + item.ERCont;
                drt["Challan"] = item.ChallanNo;
                drt["Date"] = item.Date!=null?Convert.ToDateTime(item.Date).ToString("dd/MM/yyyy"):"";
                rds.Tables["ESIHalfStatementTrn"].Rows.Add(drt);

                comp = item.Company.ESIRegistrationNumber;
                code = item.Company.Code;
            }
            double totalip = data.Sum(x => x.MonthlyCont.Sum(y => y.IPCont));
            dr["CompanyName"] = comp;
            dr["EmployeeCont"] = totalip;
            dr["EmployerCont"] = data.Sum(x=>x.ERCont);
            dr["TotalCont"] = totalip + data.Sum(x=>x.ERCont);
            dr["FY"] = fy;
            rds.Tables["ESIHalfStatement"].Rows.Add(dr);

            ReportDocument rd = new ReportDocument();
            string repoertname = "\\Reports\\ESIStatement.rpt";
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

                return File(stream, "application/pdf", "ESI-HalfYearlyStatement-" + code + ".pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult PrintStatement(string fy)
        {


            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Sr.No", typeof(string)));
            dt.Columns.Add(new DataColumn("Month", typeof(string)));
            dt.Columns.Add(new DataColumn("NoofEmp", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalESIWages", typeof(string)));
            dt.Columns.Add(new DataColumn("IPCont", typeof(string)));
            dt.Columns.Add(new DataColumn("ERCont", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalCont", typeof(string)));
            dt.Columns.Add(new DataColumn("Challan", typeof(string)));
            dt.Columns.Add(new DataColumn("Date", typeof(string)));

            string comp = null;
            string code = null;

            GridView gv = new GridView();
            ESIMonthlyContMasterRepository dal = new ESIMonthlyContMasterRepository();
            var data = esistatement.ToList();
            int i = 1;
            foreach (var item in data.OrderBy(x => x.Id))
            {
                double IPCont = item.MonthlyCont.Sum(x => x.IPCont);
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = i;
                dr["Month"] = item.MonthYear;
                dr["NoofEmp"] = item.MonthlyCont.Count();
                dr["TotalESIWages"] = Convert.ToDouble(item.TotalWages).ToString("##,###");
                dr["IPCont"] = Convert.ToDouble(IPCont).ToString("##,###"); 
                dr["ERCont"] = Convert.ToDouble(item.ERCont).ToString("##,###");
                dr["TotalCont"] = Convert.ToDouble((IPCont +item.ERCont)).ToString("##,###");
                dr["Challan"] = item.ChallanNo;
                dr["Date"] = item.Date != null ? Convert.ToDateTime(item.Date).ToString("dd/MM/yyyy") : "";
                dt.Rows.Add(dr);
                i++;

                comp = item.Company.Name + "(" + item.Company.ESIRegistrationNumber + ")";
                code = item.Company.Code;
            }
            gv.DataSource = dt;
            gv.DataBind();

            #region -- Report Filter --

            GridViewRow row1 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            GridViewRow row2 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            GridViewRow row3 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            GridViewRow row4 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell cell_1 = new TableCell();
            cell_1.Text = "Employee State Insurance Corporations";
            cell_1.Font.Bold = true;
            cell_1.HorizontalAlign = HorizontalAlign.Center;
            cell_1.ColumnSpan = 9;
            row1.Cells.Add(cell_1);

            TableCell cell_2 = new TableCell();
            cell_2.Text = "Yearly Contribution History of " + comp;
            cell_2.ColumnSpan = 9;
            cell_2.HorizontalAlign = HorizontalAlign.Center;
            row2.Cells.Add(cell_2);

            TableCell cell_3 = new TableCell();
            cell_3.Text = "Contribution";

            TableCell cell_4 = new TableCell();
            cell_4.Text = "Employee";
            cell_4.ColumnSpan = 2;

            TableCell cell_5 = new TableCell();
            cell_5.Text = "Employer";
            cell_5.ColumnSpan = 2;

            TableCell cell_6 = new TableCell();
            cell_6.Text = "Total";
            cell_6.ColumnSpan = 2;

            TableCell cell_7 = new TableCell();
            cell_7.Text = "FY";
            cell_7.ColumnSpan = 2;

            row3.Cells.Add(cell_3);
            row3.Cells.Add(cell_4);
            row3.Cells.Add(cell_5);
            row3.Cells.Add(cell_6);
            row3.Cells.Add(cell_7);

            double totalip = data.Sum(x => x.MonthlyCont.Sum(y => y.IPCont));

            TableCell cell3 = new TableCell();
            cell3.Text = "";

            TableCell cell4 = new TableCell();
            cell4.Text = totalip.ToString("##,###.00");
            cell4.ColumnSpan = 2;

            TableCell cell5 = new TableCell();
            cell5.Text = data.Sum(x => x.ERCont).ToString("##,###.00");
            cell5.ColumnSpan = 2;

            TableCell cell6 = new TableCell();
            cell6.Text = (totalip + data.Sum(x => x.ERCont)).ToString("##,###.00");
            cell6.ColumnSpan = 2;

            TableCell cell7 = new TableCell();
            cell7.Text = fy;
            cell7.ColumnSpan = 2;

            row4.Cells.Add(cell3);
            row4.Cells.Add(cell4);
            row4.Cells.Add(cell5);
            row4.Cells.Add(cell6);
            row4.Cells.Add(cell7);


            gv.Controls[0].Controls.AddAt(0, row1);
            gv.Controls[0].Controls.AddAt(1, row2);
            gv.Controls[0].Controls.AddAt(2, row3);
            gv.Controls[0].Controls.AddAt(3, row4);

            #endregion

            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "AnualESIStatement" + ".xls";
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

        #region ---IP CARD ---

        public ActionResult IPCard()
        {
            return View();
        }


        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadIP()
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
                            var _comPath = Server.MapPath(Url.Content("~/Upload/ePechanCard/")) + fileName;
                            var path = _comPath;
                            hpf.SaveAs(path);
                            messages = messages == "" ? hpf.FileName : messages + "#~#" + hpf.FileName;
                        }
                    }
                }
            }
            return Json(Convert.ToString(messages), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}