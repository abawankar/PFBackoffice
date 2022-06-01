using CrystalDecisions.CrystalReports.Engine;
using DAL.Transaction;
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
using UI.Common;
using UI.Models.Transaction;


namespace UI.Controllers.Transaction
{
    [HandleError]
    [Authorize]
    public class PayslipDataController : Controller
    {
        static List<PayslipDataModel> contlist = new List<PayslipDataModel>();
        static PayslipMasterModel bl = new PayslipMasterModel();

        // GET: PayslipData
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<PayslipMasterModel> model = new List<PayslipMasterModel>();
                PayslipMasterRepository dal = new PayslipMasterRepository();
                model = dal.GetAll().ToList();
                int count = model.Count;
                model = model.OrderByDescending(x => x.Id).ToList();
                List<PayslipMasterModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }


        public ActionResult UploadPayData()
        {
            return View();
        }

        
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile()
        {
            contlist.Clear();
            var dirPath = Server.MapPath(Url.Content("~/Upload/Temp/"));
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(dirPath);
            foreach (FileInfo fi in directory.GetFiles())
            {
                fi.IsReadOnly = false;
                fi.Delete();
            }
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

                                        PayslipDataModel bl = new PayslipDataModel();
                                        bl.Id = sr;
                                        sr++;
                                        bl.SalMonth = Convert.ToDateTime(x[(int)PayData.SalMonth].Trim());
                                        bl.EmpCode = x[(int)PayData.EmpCode].Trim();
                                        bl.CardNo = x[(int)PayData.CardNo].Trim();
                                        bl.GroupCode = x[(int)PayData.GroupCode].Trim();
                                        bl.PFNo = x[(int)PayData.PFNo].Trim();
                                        bl.UAN = x[(int)PayData.UAN].Trim();
                                        bl.ESI = x[(int)PayData.ESI].Trim();
                                        bl.Name = x[(int)PayData.Name].Trim();
                                        bl.FatherName = x[(int)PayData.FatherName].Trim();

                                        bl.WorkDay = Convert.ToInt32(x[(int)PayData.WorkDay].Trim());
                                        bl.Holiday = Convert.ToInt32(x[(int)PayData.Holiday].Trim());
                                        bl.WeekOff = Convert.ToInt32(x[(int)PayData.WeekOff].Trim());

                                        bl.Basic = Convert.ToDouble(x[(int)PayData.Basic].Trim());
                                        bl.Hra = Convert.ToDouble(x[(int)PayData.Hra].Trim());
                                        bl.Conv = Convert.ToDouble(x[(int)PayData.Conv].Trim());
                                        bl.Other = Convert.ToDouble(x[(int)PayData.Other].Trim());
                                        bl.Extra9 = Convert.ToDouble(x[(int)PayData.Extra9].Trim());
                                        bl.Extra12 = Convert.ToDouble(x[(int)PayData.Extra12].Trim());
                                        bl.Other6 = Convert.ToDouble(x[(int)PayData.Other6].Trim());
                                        bl.GrossPay = Convert.ToDouble(x[(int)PayData.GrossPay].Trim());

                                        bl.PfWorker = Convert.ToDouble(x[(int)PayData.PfWorker].Trim());
                                        bl.EsiWorker = Convert.ToDouble(x[(int)PayData.EsiWorker].Trim());
                                        bl.TDS = Convert.ToDouble(x[(int)PayData.TDS].Trim());
                                        bl.Advance = x[(int)PayData.Advance] !=""? Convert.ToDouble(x[(int)PayData.Advance].Trim()):0;
                                        bl.TotalDedn = Convert.ToDouble(x[(int)PayData.TotalDedn].Trim());
                                        bl.NetPay = Convert.ToDouble(x[(int)PayData.NetPay].Trim());

                                        bl.Designation = x[(int)PayData.Designation].Trim();
                                        bl.PAN = x[(int)PayData.PAN].Trim();

                                        bl.AppointDt = Convert.ToDateTime(x[(int)PayData.AppointDt].Trim());

                                        bl.Dept = Convert.ToInt32(x[(int)PayData.Dept].Trim());

                                        bl.Emailid = x[(int)PayData.EmilId].Trim();
                                        bl.BankName = x[(int)PayData.BankName].Trim();
                                        bl.BankAccount = x[(int)PayData.BankAccount].Trim();

                                        if (x[(int)PayData.DOB] != "")
                                            bl.DOB = Convert.ToDateTime(x[(int)PayData.DOB].Trim());

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

        [HttpPost]
        public JsonResult BulkList( int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<PayslipDataModel> model = new List<PayslipDataModel>();
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
        public JsonResult FooterData()
        {
            try
            {
                List<PayslipMasterModel> model = new List<PayslipMasterModel>();
                var data = contlist.ToList();
                bl.SalMonth = data.Select(x => x.SalMonth).FirstOrDefault();
                bl.NoofEmp = data.Count();
                bl.Basic = data.Sum(x => x.Basic);
                bl.Hra = data.Sum(x => x.Hra);
                bl.Conv = data.Sum(x => x.Conv);
                bl.Other = data.Sum(x => x.Other);
                bl.Extra9 = data.Sum(x => x.Extra9);
                bl.Extra12 = data.Sum(x => x.Extra12);
                bl.Other6 = data.Sum(x => x.Other6);
                bl.GrossPay = data.Sum(x => x.GrossPay);
                bl.PfWorker = data.Sum(x => x.PfWorker);
                bl.EsiWorker = data.Sum(x => x.EsiWorker);
                bl.TDS = data.Sum(x => x.TDS);
                bl.Advance = data.Sum(x => x.Advance);
                bl.TotalDedn = data.Sum(x => x.TotalDedn);
                bl.NetPay = data.Sum(x => x.NetPay);


                model.Add(bl);
                return Json(new { Result = "OK", Records = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Delete(PayslipMasterModel model)
        {
            //if (User.IsInRole("Admin"))
            //{ }
            //else {
            //    return Json(new { Result = "Error", Message = "You are not Authorize to do this action!" });
            //}
            try
            {
                PayslipMasterRepository dal = new PayslipMasterRepository();
                var data = dal.Delete(model.Id);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public JsonResult SaveDraft()
        {
            string message = "";
            PayslipMasterRepository dal = new PayslipMasterRepository();
            dal.Insert(bl, contlist);

            message = "Record Saved Successfully";
            contlist.Clear();

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FullDetails(int id = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            List<PayslipDataModel> model = new List<PayslipDataModel>();
            PayslipMasterRepository dal = new PayslipMasterRepository();
            model = dal.GetFullDetails(id).ToList().OrderBy(x => x.MailSent).ToList();
            int count = model.Count;
            List<PayslipDataModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
            return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
        }


        [HttpPost]
        public ActionResult SendBulkPayslip(int masterid)
        {
            string message = "";
            
            PayslipMasterRepository dal = new PayslipMasterRepository();
            var data = dal.GetFullDetails(masterid).ToList();
            

            foreach (var item in data.Where(x=>x.MailSent ==0))
            {
                try
                {
                    Reportdataset rds = new Reportdataset();
                    DataRow dr = rds.Tables["PaySlip"].NewRow();
                    dr["Id"] = item.Id;

                    dr["SalMonth"] = item.SalMonth.ToString("MMM-yyyy").ToUpper();
                    dr["EmpCode"] = item.EmpCode.ToUpper(); ;
                    dr["GroupCode"] = item.GroupCode;
                    dr["PFNo"] = item.PFNo;
                    dr["UAN"] = item.UAN;
                    dr["ESINo"] = item.ESI;
                    dr["Name"] = item.Name;
                    dr["FatherName"] = item.FatherName;
                    dr["WorkDay"] = item.WorkDay;
                    dr["Holiday"] = item.Holiday;
                    dr["WeekOff"] = item.WeekOff;
                    dr["Basic"] = item.Basic;
                    dr["Hra"] = item.Hra;
                    dr["Conv"] = item.Conv;
                    dr["Other"] = item.Other;
                    dr["Extra9"] = item.Extra9;
                    dr["Extra12"] = item.Extra12;
                    dr["Other6"] = item.Other6;
                    dr["GrossPay"] = item.GrossPay;
                    dr["PF"] = item.PfWorker;
                    dr["ESI"] = item.EsiWorker;
                    dr["TDS"] = item.TDS;
                    dr["Advance"] = item.Advance;
                    dr["TotalDedn"] = item.TotalDedn;
                    dr["NetPay"] = item.NetPay;
                    dr["Designation"] = item.Designation;
                    dr["PAN"] = item.PAN;
                    dr["Dept"] = item.Dept;
                    dr["DaysInMonth"] = System.DateTime.DaysInMonth(item.SalMonth.Year, item.SalMonth.Month);

                    dr["BankName"] = item.BankName;
                    dr["AccountNo"] = item.BankAccount;
                    dr["DOB"] = item.DOB != null ? Convert.ToDateTime(item.DOB).ToString("dd-MMM-yyyy") : "";
                    dr["DOJ"] = item.AppointDt != null ? Convert.ToDateTime(item.AppointDt).ToString("dd-MMM-yyyy") : "";

                    rds.Tables["PaySlip"].Rows.Add(dr);

                    ReportDocument rd = new ReportDocument();
                    string repoertname = "\\Reports\\Payslip.rpt";
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

                        MailMessage msg = new MailMessage();
                        msg.To.Add(new MailAddress(item.Emailid));
//                        msg.CC.Add(new MailAddress("v-venkat.raman@jublfood.com"));
                        msg.From = new MailAddress(ConfigurationManager.AppSettings["mail"], "Savvy Payslip");
                        msg.Subject = "Payslip for the month of : " + item.SalMonth.ToString("MMM-yy") + "(" + item.EmpCode + ")";
                        msg.Body = "Hi, <br/> <br/>Find attached salary slip fo the month of " + item.SalMonth.ToString("MMM-yy") + "<br/><br> Send from Savvy Payslip Portal<br>";
                        try { msg.Attachments.Add(new Attachment(stream, "Payslip-" +item.EmpCode +"-" + item.SalMonth.ToString("MMM-yy") + ".pdf")); }
                        catch (Exception) { }
                        msg.IsBodyHtml = true;
                        
                        EmailSetting.SendEmail(msg);

                        UpdateMailStatus(item.Id, 1);

                    }
                    catch(Exception ex) { throw ex; }
                }
                catch (Exception ex) { throw ex; }

            }
            message = "Sucessfully sent";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendPayslip(int id)
        {
            string message = "";

            PayslipDataRepository dal = new PayslipDataRepository();
            var item = dal.GetById(id);
            Reportdataset rds = new Reportdataset();

                try
                {

                    DataRow dr = rds.Tables["PaySlip"].NewRow();
                    dr["Id"] = item.Id;

                    dr["SalMonth"] = item.SalMonth.ToString("MMM-yyyy").ToUpper();
                    dr["EmpCode"] = item.EmpCode.ToUpper();
                    dr["GroupCode"] = item.GroupCode;
                    dr["PFNo"] = item.PFNo;
                    dr["UAN"] = item.UAN;
                    dr["ESINo"] = item.ESI;
                    dr["Name"] = item.Name;
                    dr["FatherName"] = item.FatherName;
                    dr["WorkDay"] = item.WorkDay;
                    dr["Holiday"] = item.Holiday;
                    dr["WeekOff"] = item.WeekOff;
                    dr["Basic"] = item.Basic;
                    dr["Hra"] = item.Hra;
                    dr["Conv"] = item.Conv;
                    dr["Other"] = item.Other;
                    dr["Extra9"] = item.Extra9;
                    dr["Extra12"] = item.Extra12;
                    dr["Other6"] = item.Other6;
                    dr["GrossPay"] = item.GrossPay;
                    dr["PF"] = item.PfWorker;
                    dr["ESI"] = item.EsiWorker;
                    dr["TDS"] = item.TDS;
                    dr["Advance"] = item.Advance;
                    dr["TotalDedn"] = item.TotalDedn;
                    dr["NetPay"] = item.NetPay;
                    dr["Designation"] = item.Designation;
                    dr["PAN"] = item.PAN;
                    dr["Dept"] = item.Dept;
                    dr["DaysInMonth"] = System.DateTime.DaysInMonth(item.SalMonth.Year, item.SalMonth.Month);

                    dr["BankName"] = item.BankName;
                    dr["AccountNo"] = item.BankAccount;
                    dr["DOB"] = item.DOB != null?Convert.ToDateTime(item.DOB).ToString("dd-MMM-yyyy"):"";
                    dr["DOJ"] = item.AppointDt != null ? Convert.ToDateTime(item.AppointDt).ToString("dd-MMM-yyyy") : "";

                    rds.Tables["PaySlip"].Rows.Add(dr);

                    ReportDocument rd = new ReportDocument();
                    string repoertname = "\\Reports\\Payslip.rpt";
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

                        MailMessage msg = new MailMessage();
                        msg.To.Add(new MailAddress(item.Emailid));
                        msg.From = new MailAddress(ConfigurationManager.AppSettings["mail"], "Savvy Payslip");
                        msg.Subject = "Payslip for the month of : " + item.SalMonth.ToString("MMM-yy") + "(" + item.EmpCode + ")";
                        msg.Body = "Hi, <br/> <br/>Find attached salary slip fo the month of " + item.SalMonth.ToString("MMM-yy") + "<br/><br> Send from Savvy Payslip Portal<br>";
                        try { msg.Attachments.Add(new Attachment(stream, "Payslip-" +item.EmpCode +"-" + item.SalMonth.ToString("MMM-yy") + ".pdf")); }
                        catch (Exception) { }
                        msg.IsBodyHtml = true;
                        EmailSetting.SendEmail(msg);
                        UpdateMailStatus(item.Id, 1);

                    }
                catch (Exception ex) { throw ex; }
            }
            catch (Exception ex) { throw ex; }


            message = "Sucessfully sent";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public void UpdateMailStatus(int id, int status)
        {
            PayslipDataDAL dal = new PayslipDataDAL();
            IPayslipData bl = dal.GetById(id);
            bl.MailSent = status == 0 ? MailSent.No : MailSent.Yes;
            dal.InsertOrUpdate(bl);
        }
    }
}