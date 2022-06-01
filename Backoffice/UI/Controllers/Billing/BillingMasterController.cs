using CrystalDecisions.CrystalReports.Engine;
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
using UI.Common;
using UI.Models.Billing;
using UI.Models.Master;

namespace UI.Controllers.Billing
{
    public class BillingMasterController : Controller
    {
        static List<BillTransactionModel> servicelist = new List<BillTransactionModel>();

        static List<BillMasterModel> exportlist = new List<BillMasterModel>();

        // GET: BillingMaster
        public ActionResult Index()
        {
            CompanyRepository dal = new CompanyRepository();
            ViewBag.comp = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            BillingCompanyRepository dal1 = new BillingCompanyRepository();
            ViewBag.billingcomp = new SelectList(dal1.GetAll().OrderBy(x => x.Name), "Id", "Name");

            ViewBag.status = new SelectList(new[] {
                new SelectListItem { Text = "Pending" , Value = "1" },
                new SelectListItem { Text = "Received", Value = "2" },
                new SelectListItem { Text = "Cancelled", Value = "3" },
            }, "Value", "Text");

            return View();
        }

        [HttpPost]
        public JsonResult List(int compid=0, int billcompid=0,int status=0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                BillMasterRepository dal = new BillMasterRepository();
                List<BillMasterModel> model = new List<BillMasterModel>();
                model = dal.GetAll().ToList();
                if(compid != 0)
                {
                    model = model.Where(x => x.Company.Id == compid).ToList();
                }
                if (billcompid != 0)
                {
                    model = model.Where(x => x.BillingCompany.Id == billcompid).ToList();
                }
                if (status != 0)
                {
                    model = model.Where(x => x.Status == status).ToList();
                }
                int count = model.Count;
                model = model.OrderByDescending(x => x.Id).ToList();
                List<BillMasterModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(BillMasterModel model)
        {
            //if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R009"))
            //{ }
            //else {
            //    return Json(new { Result = "Error", Message = "You are not Authorize to do this action!" });
            //}
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                BillMasterRepository dal = new BillMasterRepository();
                dal.Edit(model);
                BillMasterModel model1 = dal.GetById(model.Id);
                return Json(new { Result = "OK", Record = model1 });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult BillTrans(int billid)
        {
            try
            {
                BillMasterRepository dal = new BillMasterRepository();
                List<BillTransactionModel> model = new List<BillTransactionModel>();
                model = dal.BillTransaction(billid).ToList();
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult AddBill()
        {
            servicelist.Clear();

            CompanyRepository dal = new CompanyRepository();
            ViewBag.comp = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            BillingCompanyRepository dal1 = new BillingCompanyRepository();
            ViewBag.billingcomp = new SelectList(dal1.GetAll().OrderBy(x => x.Name), "Id", "Name");

            ViewBag.today = System.DateTime.Today.ToString("yyyy-MM-dd");
            return View();
        }

        [HttpPost]
        public ActionResult AddBill(BillMasterModel model)
        {
            IBillMaster bl = new BillMaster();
            bl.Company = new Company { Id = model.CompId };
            bl.BillingCompany = new BillingCompany { Id = model.BillingCompId };
            bl.Date = model.Date;
            bl.Status = 1;
            CompanyDAL comp = new CompanyDAL();
            string compStateCode = comp.GetById(model.CompId).StateCode;

            BillingCompanyDAL billcomp = new BillingCompanyDAL();
            string billStateCode = billcomp.GetById(model.BillingCompId).StateCode;

            ServiceNameDAL srv = new ServiceNameDAL();

            List<IBillTransaction> trnlist = new List<IBillTransaction>();
            foreach (var item in servicelist)
            {
                double taxrate = srv.GetById(item.ServiceId).Rate;

                IBillTransaction trn = new BillTransaction();
                trn.Service = new ServiceName { Id = item.ServiceId };
                trn.MonthYear = item.MonthYear;
                trn.Year = item.Year;
                trn.Amount = item.Amount;
                
                if(compStateCode == billStateCode)
                {
                    trn.CGSTRate = taxrate / 2;
                    trn.CGST = Math.Round((trn.Amount * trn.CGSTRate) / 100, 2);

                    trn.SGSTRate = taxrate / 2;
                    trn.SGST = Math.Round((trn.Amount * trn.SGSTRate) / 100, 2);
                }
                else
                {
                    trn.IGSTRate = taxrate;
                    trn.IGST = Math.Round((trn.Amount * taxrate) / 100, 2);
                }

                trnlist.Add(trn);
            }
            bl.Tran = trnlist;
            BillMasterDAL dal = new BillMasterDAL();
            bl.BillNo = (dal.GetMaxBillNumber(model.BillingCompId) + 1).ToString();
            dal.InsertOrUpdate(bl);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ServiceList()
        {
            try
            {
                var model = servicelist.ToList();
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AddService(BillTransactionModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                BillTransactionModel bl = new BillTransactionModel();
                bl.Id = servicelist.Count() == 0 ? 1 : servicelist.Count();
                bl.ServiceId = model.ServiceId;
                bl.MonthYear = model.MonthYear;
                bl.Year = model.Year;
                bl.Amount = model.Amount;
                servicelist.Add(bl);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateService(BillTransactionModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                var data = servicelist.Where(x => x.Id == model.Id).SingleOrDefault();
                
                data.ServiceId = model.ServiceId;
                data.MonthYear = model.MonthYear;
                data.Amount = model.Amount;
                data.Year = model.Year;
                return Json(new { Result = "OK", Record = data });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetBillingPeriod()
        {
            List<ServicePeriodModel> model = new List<ServicePeriodModel>();
            model.Add(new ServicePeriodModel {Id=0, Name = "* Select *" });
            model.Add(new ServicePeriodModel {Id=1, Name = "Jan" });
            model.Add(new ServicePeriodModel {Id=2, Name = "Feb" });
            model.Add(new ServicePeriodModel {Id=3, Name = "Mar" });
            model.Add(new ServicePeriodModel {Id=4, Name = "Apr" });
            model.Add(new ServicePeriodModel {Id=5, Name = "May" });
            model.Add(new ServicePeriodModel {Id=6, Name = "Jun" });
            model.Add(new ServicePeriodModel {Id=7, Name = "Jul" });
            model.Add(new ServicePeriodModel {Id=8, Name = "Aug" });
            model.Add(new ServicePeriodModel {Id=9, Name = "Sept" });
            model.Add(new ServicePeriodModel {Id=10, Name = "Oct" });
            model.Add(new ServicePeriodModel {Id=11, Name = "Nov" });
            model.Add(new ServicePeriodModel {Id=12, Name = "Dec" });
            model.Add(new ServicePeriodModel {Id=13, Name = "Jan-Mar" });
            model.Add(new ServicePeriodModel {Id=14, Name = "Apr-Jun" });
            model.Add(new ServicePeriodModel {Id=15, Name = "Jul-Sep" });
            model.Add(new ServicePeriodModel { Id = 16, Name = "Oct-Dec" });
            var data = model.Select(c => new { DisplayText = c.Name, Value = c.Name });
            try
            {
                return Json(new { Result = "OK", Options = data });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetYear()
        {
            List<ServicePeriodModel> model = new List<ServicePeriodModel>();
            model.Add(new ServicePeriodModel { Id = 0, Name = "* Select *" });
            model.Add(new ServicePeriodModel { Id = 2016, Name = "2016" });
            model.Add(new ServicePeriodModel { Id = 2017, Name = "2017" });
            model.Add(new ServicePeriodModel { Id = 2018, Name = "2018" });
            var data = model.Select(c => new { DisplayText = c.Name, Value = c.Id });
            try
            {
                return Json(new { Result = "OK", Options = data });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult PrintBill(int id)
        {
            BillMasterRepository dal = new BillMasterRepository();
            var data = dal.GetById(id);
            Reportdataset rds = new Reportdataset();
            DataRow dr = rds.Tables["BillMaster"].NewRow();
            dr["Id"] = data.Id;
            dr["BillNo"] = data.BillNo;
            dr["Date"] = data.Date.ToString("dd-MMM-yyy");
            dr["BillingCompany"] = data.BillingCompany.Name;
            dr["BillingCompAddress"] = data.BillingCompany.Address;
            dr["BillingCompStateCode"] = data.BillingCompany.StateCode;
            dr["Company"] = data.Company.Name;
            dr["CompAddress"] = data.Company.Address;
            dr["CompStateCode"] = data.Company.StateCode;
            dr["CompGST"] = data.Company.GST;
            dr["BillingCompGST"] = data.BillingCompany.GST;
            dr["TaxableAmount"] = data.Tran.Sum(x=>x.Amount);
            dr["TotalCGST"] = data.Tran.Sum(x=>x.CGST);
            dr["TotalSGST"] = data.Tran.Sum(x=>x.SGST);
            dr["TotalIGST"] = data.Tran.Sum(x => x.IGST);
            dr["GrandTotal"] = data.Tran.Sum(x=>x.Amount+x.CGST+x.SGST+x.IGST);
            dr["AmountInWords"] = NumberToWords.SpellNumber(dr["GrandTotal"].ToString());
            rds.Tables["BillMaster"].Rows.Add(dr);
            foreach (var item in data.Tran)
            {
                DataRow drt = rds.Tables["BillTran"].NewRow();
                drt["Id"] = data.Id;
                drt["ServiceName"] = item.Service.Name;
                drt["SAC"] = item.Service.SAC;
                drt["Month"] = item.MonthYear;
                drt["Year"] = item.Year;
                drt["Amount"] = item.Amount;
                drt["CGSTRate"] = item.CGSTRate;
                drt["CGST"] = item.CGST;
                drt["SGSTRate"] = item.SGSTRate;
                drt["SGST"] = item.SGST;
                drt["IGSTRate"] = item.IGSTRate;
                drt["IGST"] = item.IGST;
                
                rds.Tables["BillTran"].Rows.Add(drt);
            }
            ReportDocument rd = new ReportDocument();
            string repoertname = "\\Reports\\Bill.rpt";
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

                return File(stream, "application/pdf", "GSTInvoice" + data.Id + ".pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        #region -----  Billing Report ----

        public ActionResult BillingReport()
        {
            BillingCompanyRepository dal1 = new BillingCompanyRepository();
            ViewBag.billingcomp = new SelectList(dal1.GetAll().OrderBy(x => x.Name), "Id", "Name");

            ViewBag.status = new SelectList(new[] {
                new SelectListItem { Text = "Pending" , Value = "1" },
                new SelectListItem { Text = "Received", Value = "2" },
                new SelectListItem { Text = "Cancelled", Value = "3" },
            }, "Value", "Text");

            return View();
        }

        [HttpPost]
        public JsonResult BillingList(string dtfrom=null,string dtto=null, int billcompid = 0, int status = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                BillMasterRepository dal = new BillMasterRepository();
                List<BillMasterModel> model = new List<BillMasterModel>();
                model = dal.GetAll().ToList();
               
                if (billcompid != 0)
                {
                    model = model.Where(x => x.BillingCompany.Id == billcompid).ToList();
                }
                if (status != 0)
                {
                    model = model.Where(x => x.Status == status).ToList();
                }
                if(!string.IsNullOrEmpty(dtfrom) && !string.IsNullOrEmpty(dtto))
                {
                    model = model.Where(x => x.Date >= Convert.ToDateTime(dtfrom) && x.Date <= Convert.ToDateTime(dtto)).ToList();
                }
                int count = model.Count;
                model = model.OrderBy(x => x.Date).ToList();
                exportlist = model.ToList();
                List<BillMasterModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }


        public ActionResult Export()
        {
            //if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R010"))
            //{
            //}
            //else {
            //    return View("Security");
            //}

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Date", typeof(string)));
            dt.Columns.Add(new DataColumn("BillNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Company", typeof(string)));
            dt.Columns.Add(new DataColumn("TaxableAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("CGST", typeof(string)));
            dt.Columns.Add(new DataColumn("SGST", typeof(string)));
            dt.Columns.Add(new DataColumn("IGST", typeof(string)));
            dt.Columns.Add(new DataColumn("GrandTotal", typeof(string)));

            GridView gv = new GridView();
            EmployeeRepository dal = new EmployeeRepository();
            foreach (var item in exportlist.OrderBy(x => x.Date))
            {
                DataRow dr = dt.NewRow();
                dr["Date"] = item.Date != null ? Convert.ToDateTime(item.Date).ToString("dd/MM/yyyy") : "";
                dr["BillNo"] = item.BillNo;
                dr["Company"] = item.Company.Name;
                dr["TaxableAmount"] = item.InvAmt;
                dr["CGST"] = item.CGST;
                dr["SGST"] = item.SGST;
                dr["IGST"] = item.IGST;
                dr["GrandTotal"] = item.GrandTotal;
                dt.Rows.Add(dr);
            }

            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "BillingReport.xls";
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

    }
}